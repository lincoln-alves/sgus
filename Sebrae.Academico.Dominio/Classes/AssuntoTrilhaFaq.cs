using System.Collections;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class AssuntoTrilhaFaq : EntidadeBasica
    {
        public virtual TrilhaFaq Faq { get; set; }

        public virtual IList<TrilhaFaq> ItensFaq { get; set; }
    }
}
