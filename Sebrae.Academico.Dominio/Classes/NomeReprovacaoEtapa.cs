using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class NomeReprovacaoEtapa
    {
        public virtual int ID { get; set; }

        public virtual string Nome { get; set; }

        public virtual IList<Etapa> ListaEtapa { get; set; }

        public NomeReprovacaoEtapa()
        {
            ListaEtapa = new List<Etapa>();
        }
    }
}
