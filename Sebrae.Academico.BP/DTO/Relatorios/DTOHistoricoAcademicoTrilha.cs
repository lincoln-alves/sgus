using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOHistoricoAcademicoTrilha
    {
        public string NomeTrilha { get; set; }
        public string NivelTrilha { get; set; }
        public int CargaHoraria { get; set; }
        public DateTime? DataConclusao { get; set; }
        public double NotaFinal { get; set; } 
    }
}
