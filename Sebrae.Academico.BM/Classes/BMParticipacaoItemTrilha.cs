
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMParticipacaoItemTrilha : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<ItemTrilhaParticipacao> repositorio;

        #endregion

        #region "Construtor"

        public BMParticipacaoItemTrilha()
        {
            repositorio = new RepositorioBase<ItemTrilhaParticipacao>();
        }

        #endregion

        public void Salvar(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            ValidarParticipacaoItemTrilhaInformada(pItemTrilhaParticipacao);
            repositorio.Salvar(pItemTrilhaParticipacao);
        }
        
        private void ValidarParticipacaoItemTrilhaInformada(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            // Validando se a instância da item trilha participacao está nula.
            this.ValidarInstancia(pItemTrilhaParticipacao);

            if (pItemTrilhaParticipacao.UsuarioTrilha == null)
                throw new AcademicoException("Usuário da Trilha não foi Informado. Campo Obrigatório");

            if (pItemTrilhaParticipacao.ItemTrilha == null)
                throw new AcademicoException("O Item da Trilha não foi informado. Campo Obrigatório");

        }
        
        public ItemTrilhaParticipacao ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

    }

}