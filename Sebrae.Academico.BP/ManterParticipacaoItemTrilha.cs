using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterParticipacaoItemTrilha : BusinessProcessBase
    {
        private BMItemTrilhaParticipacao itemTrilhaParticipacao;

        public ManterParticipacaoItemTrilha()
            : base()
        {
            itemTrilhaParticipacao = new BMItemTrilhaParticipacao();
        }

        public void IncluirParticipacaoItemTrilha(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            try
            {
                itemTrilhaParticipacao.Salvar(pItemTrilhaParticipacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarParticipacaoItemTrilha(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            try
            {
                itemTrilhaParticipacao.Salvar(pItemTrilhaParticipacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public ItemTrilhaParticipacao ObterParticipacaoItemTrilhaporID(int pId)
        {
            try
            {
                return itemTrilhaParticipacao.ObterPorId(pId);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirParticipacaoItemTrilha(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            try
            {
                itemTrilhaParticipacao.Excluir(pItemTrilhaParticipacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }
    }

}