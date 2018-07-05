using System.Collections.Generic;

namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class TrilhaFormaAprendizagem: EntidadeBasica
    {
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
    }

}
