using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.ConsultarFormacaoDeMultiplicadores
{
    public class DTOConsultaFormacaoDeMultiplicadores
    {
        public int AnoAtual { get; set; }
        public int AnoAnterior { get; set; }

        public int ParticipantesGestores { get; set; }
        public int ParticipantesFormadores { get; set; }
        public int ParticipantesFacilitadores { get; set; }

        public int CapacitacoesGestores { get; set; }
        public int CapacitacoesFormadores { get; set; }
        public int CapacitacoesFacilitadores { get; set; }

        public List<DTOMetasFormacaoDeMultiplicadores> Metas { get; set; }
    }
}
