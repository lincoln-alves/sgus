using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TipoItemQuestionario : EntidadeBasica
    {
        public virtual bool? TodosEstilos { get; set; }

        public virtual IList<EstiloItemQuestionario> ListaEstilosItemQuestionario { get; set; }

        public TipoItemQuestionario()
        {
            IList<ItemQuestionario> ListaItemQuestionario = new List<ItemQuestionario>();
        }

        public virtual IList<ItemQuestionario> ListaItemQuestionario { get; set; }

        #region "Implicit Operator"

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumTipoItemQuestionario,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pTipoItemQuestionario"></param>
        /// <returns></returns>
        public static implicit operator TipoItemQuestionario(enumTipoItemQuestionario pTipoItemQuestionario)
        {
            return new TipoItemQuestionario() { ID = (int)pTipoItemQuestionario };
        }

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumTipoItemQuestionario,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pTipoItemQuestionario"></param>
        /// <returns></returns>
        public static implicit operator enumTipoItemQuestionario?(TipoItemQuestionario pTipoItemQuestionario)
        {
            enumTipoItemQuestionario? tipoItemQuestionario = null;

            if (pTipoItemQuestionario != null && pTipoItemQuestionario.ID > 0)
            {
                tipoItemQuestionario = (enumTipoItemQuestionario)pTipoItemQuestionario.ID;
            }

            return tipoItemQuestionario;
        }

        #endregion
       
    }
}
