using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class OrcamentoReembolso
    {
        public virtual int ID { get; set; }
        public virtual int Ano { get; set; }
        public virtual decimal Orcamento { get; set; }
        public virtual IList<Campo> ListaCampos { get; set; }
    }
}
