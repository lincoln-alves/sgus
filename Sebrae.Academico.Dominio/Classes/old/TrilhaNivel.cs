
using System.Collections.Generic;
namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class TrilhaNivel : EntidadeBasica
    {
        public virtual int MesesPrazo { get; set; }
        public virtual TrilhaNivel TrilhaNivelPreRequisito { get; set; }

        public virtual IList<UsuarioTrilha> ListaUsuarioTrilha { get; set; }
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }

    }
}
