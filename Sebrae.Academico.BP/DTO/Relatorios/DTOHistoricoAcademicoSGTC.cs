using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOHistoricoAcademicoSGTC
    {
        public string NomeCurso { get; set; }
        public DateTime DataConclusao { get; set; }
        public int? CargaHoraria { get; set; }
    }
}
