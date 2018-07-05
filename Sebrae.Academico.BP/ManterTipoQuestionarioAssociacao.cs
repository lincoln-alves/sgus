using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterTipoQuestionarioAssociacao : BusinessProcessBase
    {
        #region "Atributos Privados"
        
        private BMTipoQuestionarioAssociacao bmTipoQuestionarioAssociacao = null;

        #endregion

        #region "Construtor"

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterTipoQuestionarioAssociacao()
            : base()
        {
            bmTipoQuestionarioAssociacao = new BMTipoQuestionarioAssociacao();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirTipoQuestionarioAssociacao(TipoQuestionarioAssociacao pTipoQuestionarioAssociacao)
        {
            base.PreencherInformacoesDeAuditoria(pTipoQuestionarioAssociacao);
            bmTipoQuestionarioAssociacao.Salvar(pTipoQuestionarioAssociacao);
        }

        public void AlterarTipoQuestionarioAssociacao(TipoQuestionarioAssociacao pTipoQuestionarioAssociacao)
        {
            base.PreencherInformacoesDeAuditoria(pTipoQuestionarioAssociacao);
            bmTipoQuestionarioAssociacao.Salvar(pTipoQuestionarioAssociacao);
        }

        public IList<TipoQuestionarioAssociacao> ObterTodosTipoQuestionarioAssociacao()
        {
            return bmTipoQuestionarioAssociacao.ObterTodos();
        }

        public TipoQuestionarioAssociacao ObterTipoQuestionarioAssociacaoPorID(int pId)
        {
            return bmTipoQuestionarioAssociacao.ObterPorID(pId);
        }

        #endregion
    }
}
