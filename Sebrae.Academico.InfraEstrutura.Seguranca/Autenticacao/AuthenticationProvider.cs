using System.Web.Services.Protocols;

namespace Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao
{
    public class AuthenticationProviderRequest: SoapHeader
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
