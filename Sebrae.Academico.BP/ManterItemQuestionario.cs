using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterItemQuestionario : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMItemQuestionario bmItemQuestionario;

        #endregion

        #region "Construtor"

        public ManterItemQuestionario()
            : base()
        {
            bmItemQuestionario = new BMItemQuestionario();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirItemQuestionario(ItemQuestionario pItemQuestionario)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pItemQuestionario);
                bmItemQuestionario.Salvar(pItemQuestionario);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(ItemQuestionario pItemQuestionario)
        {
            base.PreencherInformacoesDeAuditoria(pItemQuestionario);
            pItemQuestionario.ListaItemQuestionarioOpcoes.ToList().ForEach(x => this.PreencherInformacoesDeAuditoria(x));
        }

        public void AlterarItemQuestionario(ItemQuestionario pItemQuestionario)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pItemQuestionario);
                bmItemQuestionario.Salvar(pItemQuestionario);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirItemQuestionario(int IdItemQuestionario)
        {
       
            try
            {
                ItemQuestionario itemQuestionario = null;

                if (IdItemQuestionario > 0)
                {
                    itemQuestionario = bmItemQuestionario.ObterPorID(IdItemQuestionario);
                }

                bmItemQuestionario.Excluir(itemQuestionario);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }
      
        public IList<ItemQuestionario> ObterTodosTrilhaNivel()
        {
            try
            {
                return bmItemQuestionario.ObterTodos();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public ItemQuestionario ObterItemQuestionarioPorID(int pId)
        {
            try
            {
                return bmItemQuestionario.ObterPorID(pId);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ValidarItemQuestionarioInformado(ItemQuestionario pItemQuestionario)
        {
            bmItemQuestionario.ValidarItemQuestionarioInformado(pItemQuestionario);
        }

        #endregion

        public IQueryable<ViewQuestionarioResposta> ObterRespostasPorQuestionario(int idQuestionario, int idTurma = 0)
        {
            return bmItemQuestionario.ObterRespostasPorQuestionario(idQuestionario, idTurma);
        }
    }
}
