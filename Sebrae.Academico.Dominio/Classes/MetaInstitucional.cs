using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MetaInstitucional: EntidadeBasica
    {
        public virtual DateTime DataInicioCiclo { get; set; }
        public virtual DateTime DataFimCiclo { get; set; }
        public virtual IList<ItemMetaInstitucional> ListaItensMetaInstitucional { get; set; }

        public MetaInstitucional()
        {
            this.ListaItensMetaInstitucional = new List<ItemMetaInstitucional>();
        }
    }
}
