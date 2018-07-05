using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CampoDivisor : EntidadeBasica
    {
        public virtual Campo Dividendo { get; set; }
        public virtual Campo Divisor { get; set; }
    }
}
