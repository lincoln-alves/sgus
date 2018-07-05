using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ModuloPreRequisito : EntidadeBasica
    {
        public virtual Modulo ModuloPai { get; set; }
        public virtual Modulo ModuloFilho { get; set; }

        public ModuloPreRequisito()
        {
            ModuloPai = new Modulo();
            ModuloFilho = new Modulo();
        }
    }
}
