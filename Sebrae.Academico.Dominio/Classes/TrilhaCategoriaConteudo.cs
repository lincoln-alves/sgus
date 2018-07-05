using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaCategoriaConteudo : EntidadeBasica
    {
        public virtual Trilha Trilha { get; set; }
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
    }
}
