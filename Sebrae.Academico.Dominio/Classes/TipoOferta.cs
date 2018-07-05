using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TipoOferta
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual IList<Oferta> ListaOferta { get; set; }
    }
}
