using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.ConsultarFormacaoDeFormadores
{
    public class DTOConsultaFormacaoDeFormadores
    {
        public int ParticipantesGestores { get; set; }
        public int ParticipantesFormadores { get; set; }
        public int ParticipantesFacilitadores { get; set; }

        public int CapacitacoesGestores { get; set; }
        public int CapacitacoesFormadores { get; set; }
        public int CapacitacoesFacilitadores { get; set; }

        public List<DTOCategoriasFormacaoDeFormadores> Categorias { get; set; }
    }
}
