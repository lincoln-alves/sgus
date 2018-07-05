using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EstiloItemQuestionario: ICloneable
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual IList<ItemQuestionario> ListaItemQuestionario { get; set; }

        #region "Implicit Operator"

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumEstiloItemQuestionario,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pEstiloItemQuestionario"></param>
        /// <returns></returns>
        public static implicit operator EstiloItemQuestionario(enumEstiloItemQuestionario pEstiloItemQuestionario)
        {
            return new EstiloItemQuestionario() { ID = (int)pEstiloItemQuestionario };
        }

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumEstiloItemQuestionario,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pEstiloItemQuestionario"></param>
        /// <returns></returns>
        public static implicit operator enumEstiloItemQuestionario?(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            enumEstiloItemQuestionario? estiloItemQuestionario = null;

            if (pEstiloItemQuestionario != null && pEstiloItemQuestionario.ID > 0)
            {
                estiloItemQuestionario = (enumEstiloItemQuestionario)pEstiloItemQuestionario.ID;
            }

            return estiloItemQuestionario;
        }


        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
