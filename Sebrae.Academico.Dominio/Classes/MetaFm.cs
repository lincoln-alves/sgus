using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MetaFm
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual int Numero { get; set; }
        public virtual IEnumerable<CategoriaConteudo> Categorias { get; set; }
    }
}
