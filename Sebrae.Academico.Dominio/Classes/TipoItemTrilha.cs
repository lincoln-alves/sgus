using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TipoItemTrilha
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }

        public virtual IList<ItemTrilha> ItensTrilha { get; set; }
    }
}
