using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioCategoriaConteudo : EntidadeBasica
    {
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
