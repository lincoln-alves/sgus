using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOHistoricoAcademicoExtracurricular
    {
        public string NomeCurso { get; set; }
        public string Instituicao { get; set; }
        public int? CargaHoraria { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}
