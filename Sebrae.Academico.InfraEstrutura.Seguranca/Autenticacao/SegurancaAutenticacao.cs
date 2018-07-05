using Sebrae.Academico.BM.Classes;


namespace Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao
{
    public class SegurancaAutenticacao
    {

        private BMUsuario usuarioBM;
        private BMFornecedor fornecedorBM;

        public SegurancaAutenticacao()
        {
        }

        public bool AutenticarUsuarioPorToken(AuthenticationTokenRequest autenticacao)
        {
            var bmUsuarioTrilha = new BMUsuarioTrilha();
            return bmUsuarioTrilha.AutenticarUsuarioPorToken(autenticacao.Token);
        }

        public bool AutenticaUsuario(AuthenticationRequest autenticacao)
        {
            usuarioBM = new BMUsuario();
            return usuarioBM.AutenticarUsuario(autenticacao.Login, autenticacao.Senha);
        }

        public bool AutenticaFornecedor(AuthenticationProviderRequest autenticacao)
        {
            fornecedorBM = new BMFornecedor();
            return fornecedorBM.AutenticarFornecedor(autenticacao.Login, autenticacao.Senha);
        }

        
    }
}
