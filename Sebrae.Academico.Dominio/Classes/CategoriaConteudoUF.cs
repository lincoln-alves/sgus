using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CategoriaConteudoUF : EntidadeBasica
    {
        public virtual CategoriaConteudo Categoria { get; set; }
        public virtual Uf UF { get; set; }
    }
}
