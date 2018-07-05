
using System.Collections.Generic;
namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class Trilha : EntidadeBasica
    {
        public virtual IList<UsuarioTrilha> ListaUsuarioTrilha { get; set; }

        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
    }
}
