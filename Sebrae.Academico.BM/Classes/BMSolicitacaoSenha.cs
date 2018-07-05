using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSolicitacaoSenha : BusinessManagerBase
    {

        private RepositorioBase<SolicitacaoSenha> repositorio;

        public BMSolicitacaoSenha()
        {
            repositorio = new RepositorioBase<SolicitacaoSenha>();
        }

        public void Salvar(SolicitacaoSenha pSenhaSolicitacao)
        {
            repositorio.Salvar(pSenhaSolicitacao);
        }

        public SolicitacaoSenha ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<SolicitacaoSenha> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        protected override bool ValidarDependencias(object pTag)
        {
            return true;
        }

        public void Excluir(SolicitacaoSenha pSenhaSolicitacao)
        {
            repositorio.Excluir(pSenhaSolicitacao);
        }

        /// <summary>
        /// Atualiza o Solicitação de senha para invalidar o token
        /// </summary>
        /// <param name="token">Token enviado ao usuário para realizar invalidação do token</param>
        public void InvalidarToken(SolicitacaoSenha pSolicitacaoSenha)
        {
            //Atualiza o Solicitação para invalidar o token
            pSolicitacaoSenha.DataValidade = DateTime.Now;
            Salvar(pSolicitacaoSenha);
        }

        public SolicitacaoSenha ObterPorToken(string token)
        {
            SolicitacaoSenha solicitacaoSenha = null;
            var query = repositorio.session.Query<SolicitacaoSenha>();

            //Token
            solicitacaoSenha = query.FirstOrDefault(x => x.Token == token.Trim());

            return solicitacaoSenha;
        }

    }
}
