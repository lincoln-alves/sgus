using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaTutorial : EntidadeBasica
    {           
        public virtual enumCategoriaTrilhaTutorial Categoria { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual int Ordem { get; set; }
    }
}
