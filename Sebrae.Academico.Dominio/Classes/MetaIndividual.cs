using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MetaIndividual : EntidadeBasica
    {
        public virtual string IDChaveExterna { get; set; }
        public virtual DateTime DataValidade { get; set; }
        public virtual IList<ItemMetaIndividual> ListaItensMetaIndividual { get; set; }
        public virtual Usuario Usuario { get; set; }

        public MetaIndividual()
        {
            this.ListaItensMetaIndividual = new List<ItemMetaIndividual>();
        }

    }
}
