using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio
{
    public class TipoQuestionario: EntidadeBasica
    {
        public TipoQuestionario()
        {
            IList<Questionario> ListaQuestionario = new List<Questionario>();
        }

        public virtual IList<Questionario> ListaQuestionario { get; set; }
    }
}
