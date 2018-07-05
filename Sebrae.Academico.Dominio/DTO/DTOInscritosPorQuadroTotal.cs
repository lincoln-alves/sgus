using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOInscritosPorQuadroTotal
    {
        public virtual string UF { get; set; }
        public virtual int Inscritos { get; set; }
        public virtual float QuadroTotal { get; set; }
    }
}
