using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioUsuarioMatriculadoSEFormaAquisicao
    {
        public string FormaAquisicao { get; set; }
        public string SolucaoEducacional { get; set; }
        public string Oferta { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public string StatusMatricula { get; set; }
        public string UFResponsavel { get; set; }
    }
}
