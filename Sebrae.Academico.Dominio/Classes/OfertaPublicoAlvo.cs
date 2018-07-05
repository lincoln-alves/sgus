using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class OfertaPublicoAlvo : EntidadeBasica
    {
        public virtual PublicoAlvo PublicoAlvo { get; set; }
        public virtual Oferta Oferta { get; set; }
    }
}
