
using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioMatriculaSolucaoEducacional
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public DateTime DataInscricao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string NivelOcupacional { get; set; }
        public string SolucaoEducacional { get; set; }
        public string Situacao { get; set; }
        public string Email { get; set; }
        public string UFResponsavel { get; set; }
    }
}
