using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterItemQuestionarioOpcoes : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMItemQuestionarioOpcoes bmItemQuestionarioOpcoes;

        #endregion

        #region "Construtor"

        public ManterItemQuestionarioOpcoes()
            : base()
        {
            bmItemQuestionarioOpcoes = new BMItemQuestionarioOpcoes();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirOpcaoDeResposta(ItemQuestionarioOpcoes pOpcaoDeResposta)
        {
            try
            {
                bmItemQuestionarioOpcoes.Salvar(pOpcaoDeResposta);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarOpcaoDeResposta(ItemQuestionarioOpcoes pOpcaoDeResposta)
        {
            try
            {
                bmItemQuestionarioOpcoes.Salvar(pOpcaoDeResposta);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirOpcaoDeResposta(int IdItemQuestionarioOpcoes)
        {
            try
            {
                ItemQuestionarioOpcoes itemQuestionarioOpcoes = null;

                if (IdItemQuestionarioOpcoes > 0)
                {
                    itemQuestionarioOpcoes = bmItemQuestionarioOpcoes.ObterPorID(IdItemQuestionarioOpcoes);
                }

                bmItemQuestionarioOpcoes.Excluir(itemQuestionarioOpcoes);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<ItemQuestionarioOpcoes> ObterTodasOpcoesDeResposta()
        {
            try
            {
                return bmItemQuestionarioOpcoes.ObterTodos();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public ItemQuestionarioOpcoes ObterOpcaoDeRespostaPorID(int pId)
        {
            try
            {
                return bmItemQuestionarioOpcoes.ObterPorID(pId);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
