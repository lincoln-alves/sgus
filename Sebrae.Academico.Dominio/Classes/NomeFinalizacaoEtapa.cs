using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class NomeFinalizacaoEtapa
    {
        public virtual int ID { get; set; }

        public virtual string Nome { get; set; }

        public virtual IList<Etapa> ListaEtapa { get; set; }

        public NomeFinalizacaoEtapa()
        {
            ListaEtapa = new List<Etapa>();
        }
    }
}
