using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ProgramaCategoriaConteudo : EntidadeBasica
    {
        public virtual Programa Programa { get; set; }
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
    }
}
