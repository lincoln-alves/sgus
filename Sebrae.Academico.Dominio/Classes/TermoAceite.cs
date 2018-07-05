using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TermoAceite : EntidadeBasica
    {
        public virtual string Texto { get; set; }
        public virtual string PoliticaConsequencia { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Uf Uf { get; set; }

        public virtual IList<TermoAceiteCategoriaConteudo> ListaCategoriaConteudo { get; set; }

        public TermoAceite()
        {
            ListaCategoriaConteudo = new List<TermoAceiteCategoriaConteudo>();
        }
    }
}
