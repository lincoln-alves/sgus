using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Dominio.Classes;
using System;

namespace Sebrae.Academico.BP.Services
{

    public abstract class BusinessProcessServicesBase: BusinessProcessBase
    {

        protected BMUsuario usuarioBM;

        public BusinessProcessServicesBase() : base() { }
        

        public bool AutenticaUsuario(AuthenticationRequest autenticacao)
        {
            usuarioBM = new BMUsuario();
            return usuarioBM.AutenticarUsuario(autenticacao.Login,autenticacao.Senha);
        }

        protected void RegistrarLogExecucaoFornecedor(Fornecedor pFornecedor, string pNomeMetodo)
        {
            new BMLogAcessoFornecedor().Salvar(new LogAcessoFornecedor()
            {
                DataAcesso = DateTime.Now,
                IDFornecedor = pFornecedor.ID,
                Metodo = pNomeMetodo
            });
        }

        
    }
}
