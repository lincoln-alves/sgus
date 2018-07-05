using System;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioMonitoramentoTurmas : BusinessProcessBaseRelatorio, IDisposable
    {
        public RelatorioMonitoramentoTurmas()
        {
            StatusExistentes = new List<enumStatusTurma>
            {
                enumStatusTurma.Prevista,
                enumStatusTurma.Confirmada,
                enumStatusTurma.Cancelada,
                enumStatusTurma.EmAndamento,
                enumStatusTurma.Realizada
            };
        }

        public List<enumStatusTurma> StatusExistentes { get; set; }

        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOMonitoramentoTurma> ObterTodosPorCategoriaConteudo(string idCategoriaConteudo = null, IEnumerable<int> pUfResponsavel = null)
        {
            return ExecutarProcedureBase("DASHBOARD_REL_MatriculasTurmas", idCategoriaConteudo, 0, pUfResponsavel);
        }

        private List<DTOMonitoramentoTurma> ExecutarProcedureBase(string procedure, string idCategoriaConteudo = null, int? ano = null, IEnumerable<int> pUfResponsavel = null)
        {
            RegistrarLogExecucao();

            return (new ManterTurma()).ExecutarProcedureBase(procedure, idCategoriaConteudo, ano, pUfResponsavel);
        }

        public IList<DTOMonitoramentoTurma> ObterTotalStatus(string idCategoriaConteudo = null, DateTime? dataInicio = null,DateTime? dataFim = null, string status = null, IEnumerable<int> pUfResponsavel = null)
        {
            return (new ManterTurma()).ObterTotalStatus(idCategoriaConteudo, dataInicio, dataFim, status, pUfResponsavel);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void InserirStatusSemDados(IList<DTOMonitoramentoTurma> dtoTurmas)
        {
            var statusNaoInseridos = new List<enumStatusTurma>();

            // Obtém os status que não estão presentes no resultado da consulta.
            statusNaoInseridos.AddRange(
                StatusExistentes.Where(
                    e => !dtoTurmas.Where(x => x.Status.HasValue).Select(x => (int)x.Status).Contains((int)e)));

            var turma = dtoTurmas.FirstOrDefault();

            foreach (var statusNaoInserido in statusNaoInseridos)
            {
                dtoTurmas.Add(new DTOMonitoramentoTurma
                {
                    Status = (int)statusNaoInserido,
                    TotalMatriculados = 0,
                    TotalMatriculadosAno = 0,
                    TotalTurmas = 0,
                    TotalTurmasAno = 0,
                    TotalTurmasComStatus = dtoTurmas.Any() ? turma != null ? turma.TotalTurmasComStatus : 0 : 0,
                    TotalMatriculasComStatus = dtoTurmas.Any() ? turma != null ? turma.TotalMatriculasComStatus : 0 : 0
                });
            }
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }
    }
}
