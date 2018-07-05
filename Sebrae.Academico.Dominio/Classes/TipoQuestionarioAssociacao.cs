using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TipoQuestionarioAssociacao : EntidadeBasica, ICloneable
    {
        public virtual IList<QuestionarioAssociacao> ListaQuestionarioAssociacao { get; set; }
        public virtual IList<QuestionarioParticipacao> ListaQuestionarioParticipacao { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
        
    }
}
