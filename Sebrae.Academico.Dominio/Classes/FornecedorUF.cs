using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class FornecedorUF : EntidadeBasica
    {
        public virtual Uf UF { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
    }
}
