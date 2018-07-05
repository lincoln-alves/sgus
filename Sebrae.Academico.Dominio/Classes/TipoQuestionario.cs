using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TipoQuestionario: ICloneable
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual IList<Questionario> ListaQuestionario { get; set; }

        #region "Implicit Operator"

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumTipoQuestionario,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pTipoQuestionario"></param>
        /// <returns></returns>
        public static implicit operator TipoQuestionario(enumTipoQuestionario pTipoQuestionario)
        {
            return new TipoQuestionario() { ID = (int)pTipoQuestionario };
        }

        /// <summary>
        /// Conversão Implicita para fazermos atribuições com enumeradores do Tipo enumTipoQuestionario,
        /// pois o C# não faz conversão implicita.
        /// </summary>
        /// <param name="pTipoQuestionario"></param>
        /// <returns></returns>
        public static implicit operator enumTipoQuestionario?(TipoQuestionario pTipoQuestionario)
        {
            enumTipoQuestionario? tipoQuestionario = null;

            if (pTipoQuestionario != null && pTipoQuestionario.ID > 0)
            {
                tipoQuestionario = (enumTipoQuestionario)pTipoQuestionario.ID;
            }

            return tipoQuestionario;
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
