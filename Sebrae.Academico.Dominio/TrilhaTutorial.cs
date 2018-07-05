using Sebrae.Academico.Dominio.Enumeracao;
using System.Collections;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaTutorial : EntidadeBasica
    {
        public virtual enumCategoriaTrilhaTutorial Categoria { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual int Ordem { get; set; }
        public virtual IEnumerable<MensagemGuia> MensagensGuia { get; set; }
    }
}
