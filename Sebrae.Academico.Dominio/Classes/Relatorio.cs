using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Relatorio: EntidadeBasica
    {
        public virtual string Link { get; set; }
        public virtual IList<LogGeracaoRelatorio> ListaLogGeracaoRelatorio { get; set; }

        public Relatorio()
            : base()
        {

        }

       
    }
}
