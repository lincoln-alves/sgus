using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOHistoricoAcademicoCursos
    {
        public string NomeCurso { get; set; }
        public string FormaAquisicao { get; set; }
        public string Fornecedor { get; set; }
        public DateTime? DataConclusao { get; set; }
        public int CargaHoraria { get; set; }
        public double MediaFinal { get; set; }
    }
}
