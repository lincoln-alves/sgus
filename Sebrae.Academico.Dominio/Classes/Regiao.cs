using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Regiao : EntidadeBasica
    {
        public virtual string SiglaRegiao { get; set; }
        public virtual IList<Uf> ListaUf { get; set; }
    }
}