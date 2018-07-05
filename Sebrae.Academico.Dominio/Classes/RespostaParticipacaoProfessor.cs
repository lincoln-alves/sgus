using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class RespostaParticipacaoProfessor : EntidadeBasicaPorId, ICloneable
    {
        public virtual Usuario Professor { get; set; }
        public virtual string Resposta { get; set; }
        public virtual QuestionarioParticipacao QuestionarioParticipacao { get; set; }
        public virtual ItemQuestionarioParticipacao ItemQuestionarioParticipacao { get; set; }
        public virtual List<RespostaParticipacaoProfessorOpcoes> ListaRespostaParticipacaoOpcoes { get; set; }

        public RespostaParticipacaoProfessor()
        {
            ListaRespostaParticipacaoOpcoes = new List<RespostaParticipacaoProfessorOpcoes>();
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
