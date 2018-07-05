using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class RespostaParticipacaoProfessorOpcoes : EntidadeBasicaPorId, ICloneable
    {
        public virtual ItemQuestionarioParticipacao ItemQuestionarioParticipacao { get; set; }
        public virtual ItemQuestionarioParticipacaoOpcoes ItemQuestionarioParticipacaoOpcoes { get; set; }
        public virtual RespostaParticipacaoProfessor RespostaParticipacaoProfessor { get; set; }
        public virtual bool? RespostaSelecionada { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
