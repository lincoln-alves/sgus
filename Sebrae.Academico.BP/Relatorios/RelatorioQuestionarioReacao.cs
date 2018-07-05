using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioQuestionarioReacao : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.QuestionarioReacao; }
        }

        public IList<SolucaoEducacional> ObterSolucaoEducacional()
        {
            using (BMSolucaoEducacional bm = new BMSolucaoEducacional())
            {
                return bm.ObterTodos().ToList();
            }
        }

        public IQueryable<Oferta> ObterOferta(int pSolucaoEducacional)
        {
            using (var bm = new BMOferta())
            {
                return bm.ObterOfertaPorSolucaoEducacional(new SolucaoEducacional { ID = pSolucaoEducacional });
            }
        }

        public IList<Turma> ObterTurma(int pOferta)
        {
            using (BMTurma bm = new BMTurma())
            {
                return bm.ObterTurmasPorOferta(new Oferta { ID = pOferta });
            }
        }

        public IList<DTOQuestionarioConsolidado> ObterQuestionarioReacao(DTOFiltroRelatorioQuestionario filtro)
        {
            return (new ManterQuestionario()).ObterQuestionarioReacao(filtro.IdSolucaoEducacional, filtro.IdOferta,
                filtro.IdTurma);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
