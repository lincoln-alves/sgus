using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DtoConcluintesPorPerfilETipo
    {
        public string Tipo
        {
            get
            {
                if (NivelOcupacional.ToUpper().Contains("ALI") ||
                    NivelOcupacional.ToLower().Contains("agente local") ||
                    NivelOcupacional.ToUpper().Contains("AD") ||
                    NivelOcupacional.ToLower().Contains("credenciado"))
                    return "Colaborador Externo";

                else
                    return "Colaborador Interno";

                //if (NivelOcupacional.ToLower().Contains("dirigente") ||
                //    NivelOcupacional.ToLower().Contains("gerente") ||
                //    NivelOcupacional.ToLower().Contains("assessor") ||
                //    NivelOcupacional.ToLower().Contains("analista") ||
                //    NivelOcupacional.ToLower().Contains("assistente") ||
                //    NivelOcupacional.ToLower().Contains("trainee") ||
                //    NivelOcupacional.ToLower().Contains("estagiário") ||
                //    NivelOcupacional.ToLower().Contains("menor aprendiz"))  
                //    return "Colaborador Interno";
            }
        }

        public string NivelOcupacional { get; set; }

        public string PorcentagemValido { get; set; }

        public int Concluintes { get; set; }
    }
}
