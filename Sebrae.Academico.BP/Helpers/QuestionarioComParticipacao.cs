using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Helpers
{
    public class QuestionarioComParticipacao : BusinessProcessBaseRelatorio, IDisposable
    {
        private readonly ManterQuestionarioParticipacao _manterQuestionarioParticipacao = new ManterQuestionarioParticipacao();
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.RelatorioPesquisa; }
        }

        public IList<DTORelQuestionarioComItemPesquisa> ListaQuestionarioPesquisa(int idCategoria = 0)
        {
            this.RegistrarLogExecucao();

            return _manterQuestionarioParticipacao.ListaQuestionario(idCategoria);
        }

        public IList<DTORelQuestionarioComItemPesquisa> ListaQuestionarioPorTurma(int turma, int professor)
        {
            this.RegistrarLogExecucao();

            return _manterQuestionarioParticipacao.ListaQuestionarioPorTurma(turma, professor);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
