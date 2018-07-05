using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios.DashBoard
{
    public class DTOMatriculasPorMes
    {
        public int MesInt { get; set; }
        public string Mes { get; set; }
        public int TotalMatriculas { get; set; }
        public int TotalAprovados { get; set; }
    }
}
