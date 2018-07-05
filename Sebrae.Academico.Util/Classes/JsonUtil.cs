using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.Util.Classes
{
    public class JsonUtil
    {
        public static T GetJson<T>(string url, string method = "GET", Dictionary<string, string> postParameters = null,
            Dictionary<string, string> headerParameters = null, Dictionary<string, string> cookieParameters = null)
        {
            var jsonStringResult = ObterJson(url, method, postParameters, headerParameters, cookieParameters);
            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<T>(jsonStringResult);
            return result;
        }

        public static string ObterJson(string url, string method = "GET",
            Dictionary<string, string> postParameters = null, Dictionary<string, string> headerParameters = null,
            Dictionary<string, string> cookieParameters = null)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = method.ToUpper();
            request.Accept = "application/json";

            if (request.Method.Equals("POST")) request.ContentType = "application/x-www-form-urlencoded";

            // Carrega os parâmetros de Header caso sejam passados
            if (headerParameters != null)
            {
                foreach (var entry in headerParameters)
                {
                    request.Headers[entry.Key] = entry.Value;
                }

                // Carrega cookies de requisição como Session ID
                if (cookieParameters != null)
                {
                    request.CookieContainer = new CookieContainer();

                    foreach (var entry in headerParameters)
                    {
                        request.CookieContainer.Add(new Uri(url), new Cookie(entry.Key, entry.Value));
                    }
                }
            }

            // Parâmetros de Post
            if (postParameters != null)
            {
                var postData = postParameters.Keys.Aggregate("",
                    (current, key) =>
                        current + (HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(postParameters[key]) + "&"));
                var data = Encoding.ASCII.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

            }
            else
            {
                request.ContentLength = 0;
            }
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;

                using (var responseStream = errorResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        reader.ReadToEnd();
                    }
                }
                throw;
            }

            return "";
        }

        public static int? DrupalRestRequest(string baseUrl, string action, string method = "GET",
            Dictionary<string, string> postParameters = null, bool isRetry = false, BMConfiguracaoSistema bmConfiguracaoSistema = null)
        {
            var session =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SessionRestPortal, bmConfiguracaoSistema).Registro.Split('=');
            var csrfConfig = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.CSRFRestPortal, bmConfiguracaoSistema);
            var csrf = csrfConfig.Registro.Split('=');

            // Header Parameters
            var headerParameters = new Dictionary<string, string>
            {
                {csrf[0], csrf[1]},
                {"Cookie", session[0] + "=" + session[1]}
            };

            // Importa o tipo de conteúdo para o Drupal
            try
            {
                var result = GetJson<DTOJsonResultDrupalImportNode>(baseUrl + action, method, postParameters,
                    headerParameters);

                if (result.error_msg != null)
                {
                    throw new AcademicoException(result.error_msg);
                }
                return result.node_id;
            }
            catch (WebException ex)
            {
                // Se for a segunda tentativa e ocorreu algum erro desiste
                if (isRetry)
                {
                    throw new AcademicoException(
                        "Não foi possível salvar/atualizar a categoria entre em contato com o suporte técnico." +
                        ex.Message);
                }

                var url = baseUrl;
                var statusCode = (int) ((HttpWebResponse) ex.Response).StatusCode;

                // A sessão caiu ou CSRF não é mais válido refaz amabas
                if (statusCode != 403 && statusCode != 401)
                    throw new AcademicoException("Código de erro " + statusCode +
                                                 " não tratado. Favor entrar em contato com o suporte.");

                bmConfiguracaoSistema = bmConfiguracaoSistema ?? new BMConfiguracaoSistema();

                // Pega um token válido para usuários anonimos
                var token = requestNewDrupalAnonymousCSRF(baseUrl);

                // Pega a configuração do Cookie de sessão no banco
                var confSession = bmConfiguracaoSistema.ObterPorID((int) enumConfiguracaoSistema.SessionRestPortal);

                var postUserLoginParameters = new Dictionary<string, string>
                {
                    {
                        "username",
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UsuarioRestPortal, bmConfiguracaoSistema).Registro
                    },
                    {
                        "password",
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.PasswordRestPortal, bmConfiguracaoSistema).Registro
                    }
                };

                var headerLoginParameters = new Dictionary<string, string>
                {
                    {"X-CSRF-Token", token}
                };

                var sessionUrl = baseUrl + "user/login";

                // Pega o Cookie de login
                try
                {
                    // Realiza o login e pega session id, session name e token logado
                    var sessionJson = GetJson<DTOJsonResultDrupalSession>(sessionUrl, "POST", postUserLoginParameters,
                        headerLoginParameters);
                    confSession.Registro = sessionJson.session_name + "=" + sessionJson.sessid;
                    bmConfiguracaoSistema.Salvar(confSession);

                    // Atualiza o CSRF com o token de login
                    csrfConfig.Registro = "X-CSRF-Token=" + sessionJson.token;
                    bmConfiguracaoSistema.Salvar(csrfConfig);

                }
                catch (WebException t)
                {
                    throw new AcademicoException(
                        "Falha no login no portal. Verifique se as configurações do sistema Usuario e Senha Rest Portal. Endpoint: " +
                        sessionUrl + "\n" + t.Message);
                }

                // Chama o método original novamente flaged as isRetry
                return DrupalRestRequest(url, action, method, postParameters, true, bmConfiguracaoSistema);
            }
        }

        /*
            Retorna um Token CSRF para a realização de novos pedidos
            **************************************************************
         *  ATENÇÃO ESSE MÉTODO NÃO RETORNA TOKENS VÁLIDOS PARA AÇÕES DE USUÁRIOS LOGADOS!
         *  PARA TOKENS LOGADOS ACESSE O REGISTRO ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.CSRFRestPortal)
         *  ESSE É OBTIDO NO LOGIN DO USUÁRIO
            **************************************************************
         */

        public static string requestNewDrupalAnonymousCSRF(String baseUrl)
        {
            // Pega o Token CSRF de usuário não logado
            var csrfUrl = baseUrl + "user/token";

            // Tenta requisitar um novo CSRF
            try
            {
                var tokenJson = GetJson<DTOJsonResultDrupalCSRF>(baseUrl + "user/token", "POST");

                return tokenJson.token;
            }
            catch (WebException t)
            {
                throw new AcademicoException(
                    "Falha na requisição de um novo CSRF endpoint: " + csrfUrl + "\n" + t.Message);
            }
        }

    }
}