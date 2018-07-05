using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioMonitoramento : BusinessProcessBaseRelatorio, IDisposable
    {
        private ManterMonitoramentoIndicadores manterMonitoramento = new ManterMonitoramentoIndicadores();

        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public DTOMonitoramento ObterHorasCredenciados(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_HORAS_CREDENCIADOS", ano);
        }

        public DTOMonitoramento ObterCapacitacaoNacionalPerCapta(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_CAPACITACAO_NA_PER_CAPTA", ano);
        }

        public DTOMonitoramento ObterCapacitacaoGeralPerCapta(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_CAPACITACAO_GERAL_PER_CAPTA", ano);
        }

        public DTOMonitoramento ObterAprovacoesCapacitacoes(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_APROVACOES_CAPACITACOES", ano);
        }

        public DTOMonitoramento ObterAprovacoesAosInscricos(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_APROVACOES_AOS_INSCRITOS", ano);
        }

        public DTOMonitoramento ObterColaboradoresNA(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_COLABORADORES_NA", ano);
        }

        public DTOMonitoramento ObterCredenciadosAprovados(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_CREDENCIADOS_APROVADOS", ano);
        }

        public DTOMonitoramento ObterSatisfacaoGeral(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_SATISFACAO_GERAL", ano);
        }

        public DTOMonitoramento ObterSatisfacaoFF(int ano)
        {
            return ExecutarProcedureBase("MONITORAMENTO_REL_SATISFACAO_FF", ano);
        }

        private DTOMonitoramento ExecutarProcedureBase(string procedure, int ano)
        {
            RegistrarLogExecucao();

            return manterMonitoramento.ExecutarProcedureBase(procedure, ano);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
