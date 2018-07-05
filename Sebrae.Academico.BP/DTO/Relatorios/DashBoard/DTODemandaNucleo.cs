using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Relatorios.DashBoard
{
    public class DTODemandaNucleo
    {
        public int IdNucleo { get; set; }
        public string Nucleo { get; set; }
        public int Vencidas { get; set; }
        public int AExpirar { get; set; }
        public int NoPrazo { get; set; }
        public int Encerradas { get; set; }
    }
}
