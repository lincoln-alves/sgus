using System;
using System.Configuration;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using log4net;
using System.Text;

namespace Sebrae.Academico.Util.Classes
{
    public class ErroUtil
    {
        #region Singleton

        public static ErroUtil Instancia
        {
            get
            {

                if (Singleton.Instance == null)
                    return new ErroUtil();
                else
                    return Singleton.Instance;

            }
        }

        private class Singleton
        {
            static Singleton() { }
            internal static readonly ErroUtil Instance = new ErroUtil();
        }


        #endregion

        #region "Propriedades que encapsulam xxx"

        //

        public string EmailDeDestinoDoErro
        {
            get
            {
                string emailDeDestinoDoErro = "";

                emailDeDestinoDoErro = System.Configuration.ConfigurationManager.AppSettings["emaildedestinodoErro"];

                return emailDeDestinoDoErro;

            }
        }


        /// <summary>
        /// Indica se o sistema deve enviar email com os erros
        /// </summary>
        public bool EnviarEmail
        {
            get
            {
                bool enviaEmail = false;

                string stringEnviaEmail = ConfigurationManager.AppSettings["enviaremailcomErros"];

                if (!string.IsNullOrWhiteSpace(stringEnviaEmail))
                {
                    if (stringEnviaEmail.Equals(Constantes.SiglaSim))
                    {
                        enviaEmail = true;
                    }
                }

                return enviaEmail;

            }
        }

        #endregion

        private readonly ILog log = LogManager.GetLogger("ConfiguracaoDeLogDeErros");

        public void TratarErro(Exception ex)
        {
            /*
             * TODO:
             * 1 - Verifificar no web.config se está configurado para enviar email, se estiver, enviar para o email do web.config (criar os 2 parâmetros)
             * 2 - Logar o erro, com todos os detalhes na pasta de logs de erro com o nome do arquivo no formato lg_yyyymmdd.log
             * 3 - propagar a exception
             */

            string mensagemDeErro = ObterMensagemDeErro(ex);

            try
            {
                if (EnviarEmail)
                {
                    //Envia o E-mail com o Erro
                    EmailUtil.Instancia.EnviarEmail(ConfigurationManager.AppSettings["emaildedestinodoErro"], "Ocorreu um Erro no Sistema", mensagemDeErro);
                }
            }
            catch
            {
                //NADA A FAZER, SEGUE O FLUXO
            }

            LogarErroEmArquivoDeTexto(mensagemDeErro);

            //Propaga a exceção
            throw ex;

        }

        public string ObterMensagemDeErro(Exception ex)
        {
            string mensagemDeErro = "";

            StringBuilder sbLogDeErros = new StringBuilder();
            sbLogDeErros.AppendLine("<br />-----------------------------------------------------------------------<br />");
            sbLogDeErros.AppendLine(string.Format("Source: {0}", ex.Source));
            sbLogDeErros.AppendLine("<br />-----------------------------------------------------------------------<br />");
            sbLogDeErros.AppendLine(string.Format("Message: {0}", ex.Message));
            sbLogDeErros.AppendLine("<br />-----------------------------------------------------------------------<br />");
            sbLogDeErros.AppendLine(string.Format("StackTrace: {0}", ex.StackTrace));
            sbLogDeErros.AppendLine("<br />-----------------------------------------------------------------------<br />");
            sbLogDeErros.AppendLine(string.Format("TargetSite: {0}", ex.TargetSite));

            if (ex.InnerException != null)
            {
                sbLogDeErros.AppendLine(string.Format("InnerException: {0}", ex.InnerException.ToString()));
            }

            sbLogDeErros.AppendLine("-----------------------------------------------------------------------");

            mensagemDeErro = sbLogDeErros.ToString();

            return mensagemDeErro;
        }

        private void LogarErroEmArquivoDeTexto(string mensagemDeErro)
        {
            //string mensagemDeErro = ObterMensagemDeErro(ex);
            log.Error(mensagemDeErro);
        }

        public void TratarErro(Exception ex, string mensagem)
        {
            /*
             * TODO:
             * 1 - Verifificar no web.config se está configurado para enviar email, se estiver, enviar para o email do web.config (criar os 2 parâmetros)
             * 2 - Logar o erro, com todos os detalhes na pasta de logs de erro com o nome do arquivo no formato lg_yyyymmdd.log
             * 3 - propagar a exception
             */
            string mensagemDeErro = ObterMensagemDeErro(ex);

            if (EnviarEmail)
            {
                //Envia o E-mail com o Erro
                EmailUtil.Instancia.EnviarEmail(EmailDeDestinoDoErro, "Ocorreu um Erro no Sistema", mensagem + mensagemDeErro);
            }

            LogarErroEmArquivoDeTexto(mensagemDeErro);

            //Propaga a exceção
            throw ex;

        }

        public void TratarErroeContuniar(Exception ex, string mensagem)
        {
            /*
             * TODO:
             * 1 - Verifificar no web.config se está configurado para enviar email, se estiver, enviar para o email do web.config (criar os 2 parâmetros)
             * 2 - Logar o erro, com todos os detalhes na pasta de logs de erro com o nome do arquivo no formato lg_yyyymmdd.log
             * 3 - propagar a exception
             */
            string mensagemDeErro = ObterMensagemDeErro(ex);

            if (EnviarEmail)
            {
                //Envia o E-mail com o Erro
                EmailUtil.Instancia.EnviarEmail(EmailDeDestinoDoErro, "Ocorreu um Erro no Sistema", mensagemDeErro);
            }

            LogarErroEmArquivoDeTexto(mensagemDeErro);

        }
    }
}
