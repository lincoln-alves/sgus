using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Net;
using System.Net.Mail;

namespace Sebrae.Academico.Util.Classes
{
    public class EmailUtil
    {

        #region "Propriedades"

        public static int PortaSMTP
        {
            get
            {
                return int.Parse(ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPPorta).Registro);
            }
        }

        public static string ServidorSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPServer).Registro;
            }
        }

        public static string UsuarioSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPUsuario).Registro;
            }
        }

        public static string SenhaSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPSenha).Registro;
            }
        }


        public static bool UsarSSLSMTP
        {
            get
            {
                return Convert.ToBoolean(ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPUsarSSL).Registro);
            }
        }

        public static string RemetenteSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPRemetente).Registro;
            }
        }

        public static string NomeRemetenteSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPRemetenteNome).Registro;
            }
        }

        public static string CodificacaoSMTP
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.SMTPCodificacao).Registro;
            }
        }

        public static string EmailErroSistema
        {
            get
            {
                return ListaConfiguracaoSistema.FirstOrDefault(x => x.ID == (int)enumConfiguracaoSistema.EmailErroSistema).Registro;
            }
        }

        #endregion

        private static IList<ConfiguracaoSistema> ListaConfiguracaoSistema
        {
            get
            {
                return listaConfiguracaoSistema;
            }
        }

        private static IList<ConfiguracaoSistema> listaConfiguracaoSistema = null;

        private EmailUtil()
        {
            listaConfiguracaoSistema = ConfiguracaoSistemaUtil.ObterTodasAsInformacoesDoTemplate();
        }


        #region Singleton

        public static EmailUtil Instancia
        {
            get
            {

                if (Singleton.Instance == null)
                    return new EmailUtil();
                else
                    return Singleton.Instance;

            }
        }

        private class Singleton
        {
            static Singleton() { }
            internal static readonly EmailUtil Instance = new EmailUtil();
        }


        #endregion

        public void EnviarEmail(string emailDestinatario, string assunto, string mensagem)
        {
            try
            {
                var sc = new SmtpClient
                {
                    UseDefaultCredentials = false,
                    Host = ServidorSMTP,
                    Port = PortaSMTP,
                    EnableSsl = UsarSSLSMTP
                };
                if (!string.IsNullOrEmpty(SenhaSMTP))
                {
                    var nc = new NetworkCredential(UsuarioSMTP, SenhaSMTP);
                    sc.Credentials = nc;
                }

                if (string.IsNullOrWhiteSpace(RemetenteSMTP))
                    throw new AcademicoException("Remetente SMTP não informado em Configurações do Sistema.");

                if (string.IsNullOrWhiteSpace(NomeRemetenteSMTP))
                    throw new AcademicoException("Nome do remetente SMTP não informado em Configurações do Sistema.");

                CommonHelper.EnviarEmail(emailDestinatario, assunto, mensagem, RemetenteSMTP, NomeRemetenteSMTP, sc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public  void EnviarEmailInformandoErro(string assunto, string mensagem)
        //{
        //    try
        //    {
        //        EnviarEmail(EmailErroSistema, assunto, mensagem);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Nao fazer nada
        //    }
        //}

        ////private EmailFacade()
        ////{

        ////}

        ////TODO -> NARDO. Melhorar isso
        //public void NotificaErro(Exception ex)
        //{ 
        //    //Pega a exceção e Envia por E-mail
        //    try
        //    {
        //        EnviarEmail(EmailErroSistema, "Ocorreu um Erro no Sistema", ex.Message);
        //    }
        //    catch 
        //    {
        //        //throw ex;
        //    }
        //}

    }
}
