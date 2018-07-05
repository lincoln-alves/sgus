
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterQuestionarioParticipacao : BusinessProcessBase
    {
        private BMQuestionarioParticipacao questionarioParticipacao = null;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterQuestionarioParticipacao()
            : base()
        {
            questionarioParticipacao = new BMQuestionarioParticipacao();
        }

        public IList<DTORelQuestionarioComItemPesquisa> ListaQuestionarioPorTurma(int turma, int professor) {
            IDictionary<string, object> lstParametro = new Dictionary<string, object>();
            lstParametro.Add("id_turma", turma);
            lstParametro.Add("id_professor", professor);

            return questionarioParticipacao.ExecutarProcedure<DTORelQuestionarioComItemPesquisa>("SP_REL_QUESTIONARIO_POR_TURMA", lstParametro);
        }

        public IList<DTORelQuestionarioComItemPesquisa> ListaQuestionario(int idCategoria = 0) {
            IDictionary<string, object> lstParametro = new Dictionary<string, object>();
            lstParametro.Add("id_categoria", idCategoria);

            return questionarioParticipacao.ExecutarProcedure<DTORelQuestionarioComItemPesquisa>("SP_REL_QUESTIONARIO_COM_ITEM_PESQUISA", lstParametro);
        } 

        /// <summary>
        /// Obtém as últimas provas do usuário.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdTrilhaNivel"></param>
        /// <returns>Lista de provas do usuário</returns>
        public IList<QuestionarioParticipacao> ObterProvasDaTrilhaDoUsuario(int IdUsuario, int IdTrilhaNivel)
        {
            return questionarioParticipacao.ObterProvasDaTrilhaDoUsuario(IdUsuario, IdTrilhaNivel);
        }

        /// <summary>
        /// Obtém as últimas provas do usuário.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdTrilhaNivel"></param>
        /// <returns>Lista de provas do usuário</returns>
        public IList<QuestionarioParticipacao> ObterProvasDaTurmaDoUsuario(int IdUsuario, int IdTurma)
        {
            return questionarioParticipacao.ObterProvasDaTurmaDoUsuario(IdUsuario, IdTurma);
        }

        public IList<QuestionarioParticipacao> ObterQuestionariosPorMatriculaTurma(int id)
        {
            return
                questionarioParticipacao.ObterTodosQuestionariosComParticipacaoQueryble()
                    .Where(x => x.MatriculaTurma.ID == id)
                    .ToList();
        }

        public QuestionarioParticipacao ObterQuestionarioParticipacaoPorId(int idQuestionarioParticipacao)
        {
            return questionarioParticipacao.ObterPorID(idQuestionarioParticipacao);
        }

        public void Salvar(QuestionarioParticipacao participacao)
        {
            questionarioParticipacao.Salvar(participacao);
        }

        public void Remover(QuestionarioParticipacao participacao)
        {
            questionarioParticipacao.Remover(participacao);
        }

        public IQueryable<QuestionarioParticipacao> ObterPorQuestionarioPorFiltro(DTOFiltroRelatorioQuestionario filtro)
        {
            return questionarioParticipacao.ObterPorQuestionarioPorFiltro(filtro.IdQuestionario, filtro.IdsCategorias,
                filtro.IdSolucaoEducacional, filtro.IdOferta, filtro.IdTurma, filtro.IdsUf, filtro.IdsNivelOcupacional,
                filtro.IdsStatusMatricula);
        }

        public IQueryable<ItemQuestionarioParticipacao> ListaItemQuestionarioParticipacao(int idQuestionarioParticipacao)
        {
            return questionarioParticipacao.ListaItemQuestionarioParticipacao(idQuestionarioParticipacao);
        }

        public IQueryable<QuestionarioParticipacao> ObterTodosIQueryable()
        {
            return questionarioParticipacao.ObterTodosIQueryable();
        }
    }
}
