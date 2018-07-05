using System.IO;
using System.Web;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{

    internal class IniFile
    {
        public string Path { get; set; }


        public IniFile()
        {

        }

        public IniFile(string pPath)
        {
            this.Path = pPath;
        }

        internal string GetParameterValue(string Secao, string Chave)
        {
            StreamReader sr = new StreamReader(this.Path);
            string tsr = sr.ReadLine();
            string result = string.Empty;


            while (tsr.IndexOf("[" + Secao + "]") <= -1)
                tsr = sr.ReadLine();

            tsr = sr.ReadLine();

            while ((tsr.IndexOf("[") <= -1) && (tsr.IndexOf("]") <= -1) || (sr.EndOfStream))
            {
                
                string[] arst = tsr.Split('=');
                if (arst[0] == Chave)
                {
                    result = arst[1];
                    break;
                }

                tsr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();

            return result;
        }
    }

    public class ParametrosSistema
    {
 
        public static string GetParametro(string Secao, string Chave)
        {

            string nomeArquivo = HttpContext.Current.Server.MapPath("~") + "/bin/configuracao.ini";
            IniFile iniFile = new IniFile(nomeArquivo);

            
            return iniFile.GetParameterValue(Secao,Chave);

        }

        public static string HostSMTP
        {
            get
            {
                return GetParametro("EMAIL", "HOSTSMTP");
            }
        }

        public static int PortaSMTP
        {
            get
            {
                return int.Parse(GetParametro("EMAIL", "PORTASMTP"));
            }
        }

        public static string UsuarioSMTP
        {
            get
            {
                return GetParametro("EMAIL", "USUARIOSMTP");
            }
        }

        public static string PasswordSMTP
        {
            get
            {
                return GetParametro("EMAIL", "PASSWORDSMTP");
            }
        }

        public static int DiasFinalAbandono
        {
            get
            {
                return int.Parse(GetParametro("ABANDONO","DIASABANDONO"));
            }
        }

        public static int MesesIntervaloProvaTrilha
        {
            get
            {
                return int.Parse(GetParametro("PROVA_TRILHA", "MES_INTERVALO"));
            }
        }

        public static int QuantidadeMaximaParticipacaoTrilha
        {
            get
            {
                return int.Parse(GetParametro("PROVA_TRILHA","QTD_PARTICIPACAO"));
            }
        }


        public static int QuantidadeItensListaRanking
        {
            get
            {
                return int.Parse(GetParametro("RANKING", "QTD_ITENSLISTA"));
            }
        }

        public static string CaminhoDiretorioUpload
        {
            get
            {
                return GetParametro("SISTEMA", "CAMINHO_DIRETORIO_UPLOAD");
            }
        }

        public static int LimiteAlertaInscricaoOferta 
        {
            get 
            {
                return int.Parse(GetParametro("MATRICULAOFERTA","LIMITE_ALERTA_OFERTA"));
            }
        }

        public static string TemplateAlertaLimiteOferta
        {
            get
            {
                return GetParametro("MATRICULAOFERTA", "TEMPLATE_NOTIFICACAO_LIMITE_OFERTA");
            }
        }


        public static string EmailRemetentePadrao
        {

            get
            {
                return GetParametro("EMAIL", "REMETENTE");
            }
        }

        public static string LinkAcessoEnderecoPortal
        {
            get
            {
                return GetParametro("SISTEMA", "ENDERECO_PORTAL");
            }
        }
        
    }
}
