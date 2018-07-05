using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterSolicitacaoSenha : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMSolicitacaoSenha bmSolicitacaoSenha = null;

        #endregion

        #region "Construtor"

        public ManterSolicitacaoSenha()
            : base()
        {
            bmSolicitacaoSenha = new BMSolicitacaoSenha();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirSolicitacaoSenha(SolicitacaoSenha pSolicitacaoSenha)
        {
            bmSolicitacaoSenha.Salvar(pSolicitacaoSenha);
        }

        public void AlterarSolicitacaoSenha(SolicitacaoSenha pSolicitacaoSenha)
        {
            bmSolicitacaoSenha.Salvar(pSolicitacaoSenha);
        }

        public void ExcluirPerfil(int IdSolicitacaoSenha)
        {
            try
            {
                SolicitacaoSenha solicitacaoSenha = null;

                if (IdSolicitacaoSenha > 0)
                {
                    solicitacaoSenha = bmSolicitacaoSenha.ObterPorID(IdSolicitacaoSenha);
                }

                bmSolicitacaoSenha.Excluir(solicitacaoSenha);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<SolicitacaoSenha> ObterTodosPerfis()
        {
            return bmSolicitacaoSenha.ObterTodos();
        }

        public SolicitacaoSenha ObterSolicitacaoSenhaPorID(int pId)
        {
            return bmSolicitacaoSenha.ObterPorID(pId);
        }

        public void VerificarVigenciaDoToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new AcademicoException("O Token é Obrigatório");
                }

                SolicitacaoSenha solSenha = bmSolicitacaoSenha.ObterPorToken(token);

                if (solSenha == null)
                {
                    throw new AcademicoException("Token Inválido");
                }

                if (DateTime.Today > solSenha.DataValidade)
                {
                    throw new AcademicoException("Token Expirado. Por favor, Clique na opção Recuperar Senha, para que um novo Token seja gerado.");
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza o Solicitação de senha para invalidar o token
        /// </summary>
        /// <param name="token">Token enviado ao usuário para realizar invalidação do token</param>
        public void InvalidarToken(SolicitacaoSenha pSolicitacaoSenha)
        {
            BMSolicitacaoSenha bmSolicitacaoSenha = new BMSolicitacaoSenha();
            SolicitacaoSenha solicitacaoSenha = bmSolicitacaoSenha.ObterPorToken(pSolicitacaoSenha.Token);
            base.PreencherInformacoesDeAuditoria(solicitacaoSenha);
            bmSolicitacaoSenha.InvalidarToken(solicitacaoSenha);
        }


        #endregion
    }
}
