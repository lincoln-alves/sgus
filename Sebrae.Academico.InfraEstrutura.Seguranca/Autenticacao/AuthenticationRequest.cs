using System.Web.Services.Protocols;

namespace Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao
{
    public class AuthenticationRequest: SoapHeader
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string IP { get; set; }
        public string SessionID { get; set; }
    }

    public class AuthenticationTokenRequest : SoapHeader
    {
        public string Token { get; set; }
        public string IP { get; set; }
        public string SessionID { get; set; }
    }
}
