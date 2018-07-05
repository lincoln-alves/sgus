using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioMonitoramentoTurmas : BusinessProcessBaseRelatorio<DTOMonitoramentoTurma>, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOMonitoramentoTurma> ObterTodosPorCategoriaConteudo(string idCategoriaConteudo = null)
        {
            return ExecutarProcedureBase("DASHBOARD_REL_MatriculasTurmas", idCategoriaConteudo);
        }

        private List<DTOMonitoramentoTurma> ExecutarProcedureBase(string procedure, string idCategoriaConteudo = null)
        {
            RegistrarLogExecucao();

            var listPrm = new Dictionary<string, object> { { "idCategoriaConteudo", idCategoriaConteudo } };

            return ExecutarProcedure(procedure, listPrm).ToList();
        }

        public IList<DTOMonitoramentoTurma> ObterTotalStatus(string idCategoriaConteudo = null)
        {
            return ExecutarProcedureBase("DASHBOARD_REL_MatriculasTurmasTotais", idCategoriaConteudo);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
