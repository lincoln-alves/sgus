using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterRespostaParticipacaoProfessor : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMRespostaParticipacaoProfessor bmRespostaParticipacaoProfessor;

        #endregion

        #region "Construtor"

        public ManterRespostaParticipacaoProfessor()
            : base()
        {
            bmRespostaParticipacaoProfessor = new BMRespostaParticipacaoProfessor();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirRespostaParticipacaoProfessor(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pRespostaParticipacaoProfessor);
                bmRespostaParticipacaoProfessor.Salvar(pRespostaParticipacaoProfessor);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void PreencherInformacoesDeAuditoria(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            base.PreencherInformacoesDeAuditoria(pRespostaParticipacaoProfessor);
        }

        public void AlterarRespostaParticipacaoProfessor(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(pRespostaParticipacaoProfessor);
                bmRespostaParticipacaoProfessor.Salvar(pRespostaParticipacaoProfessor);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirRespostaParticipacaoProfessor(int IdRespostaParticipacaoProfessor)
        {
       
            try
            {
                RespostaParticipacaoProfessor respostaParticipacaoProfessor = null;

                if (IdRespostaParticipacaoProfessor > 0)
                {
                    respostaParticipacaoProfessor = bmRespostaParticipacaoProfessor.ObterPorID(IdRespostaParticipacaoProfessor);
                }

                bmRespostaParticipacaoProfessor.Excluir(respostaParticipacaoProfessor);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public RespostaParticipacaoProfessor ObterRespostaParticipacaoProfessorPorID(int pId)
        {
            try
            {
                return bmRespostaParticipacaoProfessor.ObterPorID(pId);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }
        public List<RespostaParticipacaoProfessor> ObterRespostaParticipacaoProfessorPorFiltro(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            try
            {
                return bmRespostaParticipacaoProfessor.ObterPorFiltro(pRespostaParticipacaoProfessor).ToList();
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ValidarItemQuestionarioInformado(RespostaParticipacaoProfessor pRespostaParticipacaoProfessor)
        {
            bmRespostaParticipacaoProfessor.ValidarItemQuestionarioInformado(pRespostaParticipacaoProfessor);
        }

        #endregion

        public IQueryable<RespostaParticipacaoProfessor> ObterTodosIQueryable()
        {
            return bmRespostaParticipacaoProfessor.ObterTodosIQueryable();
        }

        public IQueryable<RespostaParticipacaoProfessor> ObterPorItemQuestionarioParticipacao(
            ItemQuestionarioParticipacao itemQuestionarioParticipacao)
        {
            return
                bmRespostaParticipacaoProfessor.ObterTodosIQueryable()
                    .Where(x => x.ItemQuestionarioParticipacao.ID == itemQuestionarioParticipacao.ID);
        }

        public void Salvar(RespostaParticipacaoProfessor rpp)
        {
            bmRespostaParticipacaoProfessor.Salvar(rpp);
        }
    }
}
