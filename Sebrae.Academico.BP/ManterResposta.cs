using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterResposta : BusinessProcessBase
    {
        private BMItemQuestionarioOpcoes bmItemQuestionarioOpcoes;

        public ManterResposta()
            : base()
        {
            bmItemQuestionarioOpcoes = new BMItemQuestionarioOpcoes();
        }

        public void IncluirRespostaDoItem(ItemQuestionarioOpcoes pItemQuestionarioOpcoes)
        {
            try
            {
                bmItemQuestionarioOpcoes.Salvar(pItemQuestionarioOpcoes);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarRespostaDoItem(ItemQuestionarioOpcoes pItemQuestionarioOpcoes)
        {
            try
            {
                bmItemQuestionarioOpcoes.Salvar(pItemQuestionarioOpcoes);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirRespostaDoItem(int IdItemQuestionarioOpcoes)
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


        public IList<ItemQuestionarioOpcoes> ObterTodasAsRespostas()
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

        public ItemQuestionarioOpcoes ObterRespostaPorID(int pId)
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
           
     
    }
}
