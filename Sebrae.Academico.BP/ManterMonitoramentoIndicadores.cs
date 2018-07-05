using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;

namespace Sebrae.Academico.BP
{
    public class ManterMonitoramentoIndicadores : BusinessProcessBase
    {

        #region "Atributos Privados"

        private BMMonitoramentoIndicadores bmMonitoramentoIndicador = null;

        #endregion

        #region "Construtor"

        public ManterMonitoramentoIndicadores()
            : base()
        {
            bmMonitoramentoIndicador = new BMMonitoramentoIndicadores();
        }

        #endregion

        #region "Métodos Públicos"

        public DTOMonitoramento ExecutarProcedureBase(string procedure, int ano) {
            var listPrm = new Dictionary<string, object> { { "Ano", ano } };
            var bmMonitoramento = new BMMonitoramentoIndicadores();
            return bmMonitoramento.ExecutarProcedure<DTOMonitoramento>(procedure, listPrm).FirstOrDefault();
        }

        public void AtualizarMonitoramentoIndicador(MonitoramentoIndicadores pMonitoramentoIndicador)
        {
            base.PreencherInformacoesDeAuditoria(pMonitoramentoIndicador);
            bmMonitoramentoIndicador.Salvar(pMonitoramentoIndicador);
        }

        public void IncluirMonitoramentoIndicador(MonitoramentoIndicadores pMonitoramentoIndicador)
        {
            base.PreencherInformacoesDeAuditoria(pMonitoramentoIndicador);
            bmMonitoramentoIndicador.Salvar(pMonitoramentoIndicador);
        }

        public void IncluirMonitoramentoIndicador(IList<MonitoramentoIndicadores> pListaMonitoramentoIndicadores)
        {
            pListaMonitoramentoIndicadores.ToList().ForEach(x => base.PreencherInformacoesDeAuditoria(x));
            bmMonitoramentoIndicador.Salvar(pListaMonitoramentoIndicadores);
        }

        public MonitoramentoIndicadores ObterMonitoramentoIndicadorPorID(int pId)
        {
            return bmMonitoramentoIndicador.ObterPorID(pId);
        }

        public IList<MonitoramentoIndicadores> ObterTodosMonitoramentosIndicadores()
        {
            return bmMonitoramentoIndicador.ObterTodos();
        }
        public IList<MonitoramentoIndicadores> ObterTodosMonitoramentosIndicadoresPorFiltro(MonitoramentoIndicadores pMonitoramentoIndicador)
        {
            return bmMonitoramentoIndicador.ObterPorFiltro(pMonitoramentoIndicador).ToList();
        }

        public void Excluir(int id) {
            bmMonitoramentoIndicador.Excluir(id);
        }

        public string ObterValorIndicador(int ano, string indicador = null)
        {
            string valorIndicador = "0";

            var monitoramentoIndicadores = bmMonitoramentoIndicador.ObterPorAno(ano);
            if (monitoramentoIndicadores != null)
            {
                var monitoramentoIndicadoresValores = new BMMonitoramentoIndicadoresValores().ObterPorIndicadorAno(monitoramentoIndicadores.ID, indicador);
                if (monitoramentoIndicadoresValores != null)
                {
                    valorIndicador = monitoramentoIndicadoresValores.Descricao;
                }
            }
            
            return valorIndicador;
        }
        #endregion
    }
}
