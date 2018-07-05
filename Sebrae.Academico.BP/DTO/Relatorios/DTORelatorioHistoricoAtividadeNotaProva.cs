using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioHistoricoAtividadeNotaProva
    {
        public DateTime DataGeracaoProva { get; set; }
        public DateTime? DataParticipacaoProva { get; set; }
        public decimal NotaProva { get; set; }
    }
}
