using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioUsuarioMatriculadoPorTurma
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public string SolucaoEducacional { get; set; }
        public string Oferta { get; set; }
        public int CargaHoraria { get; set; }
        public bool TemMaterial { get; set; }
        public string FormaAquisicao { get; set; }
        public string Turma { get; set; }
        public string TipoTutoria { get; set; }
        public string Professor { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public string StatusMatricula { get; set; }
        public string Fornecedor { get; set; }
        public string UFResponsavel { get; set; }

    }
}
