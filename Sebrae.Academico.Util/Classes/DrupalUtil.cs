using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Util.Classes
{
    public class DrupalUtil
    {
        public static Dictionary<string, string> InitPostParameters(int id, string nome, string apresentacao,
            string type, bool status = true)
        {
            var result = new Dictionary<string, string>
            {
                {"data[status]", (status ? 1 : 0).ToString()},
                {"data[title]", nome},
                {"sgus_id", id.ToString()},
                {"type", type}
            };

            if (!string.IsNullOrEmpty(apresentacao)) result.Add("data[body]", apresentacao);

            return result;
        }

        public static int? NodeDrupalRest(LogSincronia logSincronia, BMConfiguracaoSistema bmConfiguracaoSistema = null)
        {
            var postParameters = logSincronia.GetDictionaryPostParameters();

            var result = JsonUtil.DrupalRestRequest(logSincronia.Url, logSincronia.Action, logSincronia.Method,
                postParameters, false, bmConfiguracaoSistema);

            if (!result.HasValue)
            {
                throw new Exception("DrupalRestRequest retornou false.");
            }
            return result;
        }

        public static void RemoverNodeDrupalRest(int id, bool salvarLog = true)
        {
            var basePath = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UrlRestPortal).Registro;
            var pathRestPortal =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.PathRestPortal).Registro;
            var logSincronia = new LogSincronia
            {
                Usuario = (new BMUsuario()).ObterUsuarioLogado(),
                Sincronizado = false,
                Url = basePath + pathRestPortal,
                Action = "delete",
                Method = "POST",
                Acao = enumAcao.Remover
            };
            var postParameters = new Dictionary<string, string>
            {
                {"node_id", id.ToString()}
            };
            logSincronia.ListaPostParameters =
                postParameters.Select(
                    p =>
                        new LogSincroniaPostParameters
                        {
                            LogSincronia = logSincronia,
                            Descricao = p.Value,
                            Registro = p.Key
                        }).ToList();
            logSincronia.Hash = logSincronia.HashObj();

            try
            {
                NodeDrupalRest(logSincronia);
            }
            catch (Exception)
            {
                if (!salvarLog)
                    throw;

                //dll UTIL não tem acesso a camada "BP" Para Evitar "referencing loop detected"
                var bmLogSincronia = new BMLogSincronia();

                var tmpLogSincronia = bmLogSincronia.ObterPorFiltro(logSincronia);

                if (tmpLogSincronia != null && tmpLogSincronia.Sincronizado)
                    return;

                bmLogSincronia.Salvar(logSincronia);
            }
        }

        public static int? SalvaNodeDrupalRest(Dictionary<string, string> postParameters, bool salvarLog = true, BMConfiguracaoSistema bmConfiguracaoSistema = null, BMLogSincronia bmLogSincronia = null, Usuario usuarioLogado = null)
        {
            var basePath = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UrlRestPortal, bmConfiguracaoSistema).Registro;

            var pathRestPortal =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.PathRestPortal, bmConfiguracaoSistema).Registro;

            var logSincronia = new LogSincronia
            {
                Usuario = usuarioLogado ?? new BMUsuario().ObterUsuarioLogado(),
                Sincronizado = false,
                Url = basePath + pathRestPortal,
                Action = "import",
                Method = "POST",
                Acao = enumAcao.Atualizar
            };

            logSincronia.ListaPostParameters =
                postParameters.Select(
                    p =>
                        new LogSincroniaPostParameters
                        {
                            LogSincronia = logSincronia,
                            Descricao = p.Value,
                            Registro = p.Key
                        }).ToList();

            logSincronia.Hash = logSincronia.HashObj();

            try
            {
                return NodeDrupalRest(logSincronia, bmConfiguracaoSistema);
            }
            catch 
            {
                if (!salvarLog)
                    throw;

                //dll UTIL não tem acesso a camada "BP" Para Evitar "referencing loop detected"
                bmLogSincronia = bmLogSincronia ?? new BMLogSincronia();

                var tmpLogSincronia = bmLogSincronia.ObterPorFiltro(logSincronia);

                if (tmpLogSincronia != null && tmpLogSincronia.Sincronizado)
                    return null;

                bmLogSincronia.Salvar(logSincronia);

                return null;
            }
        }

        public static void PermissoesUf(IList<int> lista, ref Dictionary<string, string> postParameters)
        {
            ArrayParams("data[permissoes_uf]", lista, ref postParameters);
        }

        public static void PermissoesPerfil(IList<int> lista, ref Dictionary<string, string> postParameters)
        {
            ArrayParams("data[permissoes_perfil]", lista, ref postParameters);
        }

        public static void PermissoesNivelOcupacional(IList<int> lista, ref Dictionary<string, string> postParameters)
        {
            ArrayParams("data[permissoes_nivel_ocupacional]", lista, ref postParameters);
        }

        public static void PermissoesAreaTematica(IList<int> lista, ref Dictionary<string, string> postParameters)
        {
            if (lista == null || lista.Count <= 0)
                throw new Exception("Selecione uma área temática para a solução educacional");
            ArrayParams("data[field_tema_sgus_id]", lista, ref postParameters);
        }

        public static void ArrayParams(string paramName, IList<int> lista, ref Dictionary<string, string> postParameters)
        {
            for (var i = 0; i < lista.Count; i++)
            {
                var id = lista[i];
                postParameters.Add(paramName + "[" + i + "]", id.ToString());
            }
        }
    }
}