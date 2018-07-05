using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOMonitoramentoTrilhas
    {
        public int IdTrilha { get; set; }
        public int IdTrilhaNivel { get; set; }
        public string Monitor { get; set; }
        public string CPF { get; set; }
        public string Trilha { get; set; }
        public string NivelTrilha { get; set; }
        public int QtdAcessoMonitorPeriodo { get; set; }
        public DateTime? HoraDiaUltimoAcessoMonitor { get; set; }

        public int QtdParticipacaoSEAnalisadasMonitorPeriodo { get; set; }
        public int QtdParticipacoesSEPrazoVencido { get; set; }
        public int QtdParticipacoesSEPrazoVigente { get; set; }
        public int QtdParticipacoesSEEmRevisao { get; set; }
        public int QtdParticipacoesSEPendente { get; set; }
        public int QtdParticipacoesSEAprovadas { get; set; }

        public int QtdParticipacaoSEAIAnalisadasMonitorPeriodo { get; set; }
        public int QtdParticipacoesSEAIPrazoVencido { get; set; }
        public int QtdParticipacoesSEAIPrazoVigente { get; set; }
        public int QtdParticipacoesSEAIEmRevisao { get; set; }
        public int QtdParticipacoesSEAIPendente { get; set; }
        public int QtdParticipacoesSEAIAprovadas { get; set; }

        public int QtdParticipacaoSprintAnalisadasMonitorPeriodo { get; set; }
        public int QtdParticipacoesSprintPrazoVencido { get; set; }
        public int QtdParticipacoesSprintPrazoVigente { get; set; }
        public int QtdParticipacoesSprintEmRevisao { get; set; }
        public int QtdParticipacoesSprintPendente { get; set; }
        public int QtdParticipacoesSprintAprovadas { get; set; }

        public int TotalDeParticipacoesVinculadasMonitor { get; set; }

        public string HoraDiaUltimoAcessoMonitorFormatado {
            get { return HoraDiaUltimoAcessoMonitor.HasValue ? HoraDiaUltimoAcessoMonitor.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""; }
        }
    }
}
