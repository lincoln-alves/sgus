using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemQuestionarioParticipacaoOpcoes : EntidadeBasica, ICloneable
    {
        public virtual ItemQuestionarioParticipacao ItemQuestionarioParticipacao { get; set; }
        public virtual bool? RespostaSelecionada { get; set; }
        public virtual bool? RespostaCorreta { get; set; }
        public virtual enumTipoDiagnostico TipoDiagnostico { get; set; }
        public virtual byte? OpcaoInt { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ItemQuestionarioParticipacaoOpcoes OpcaoVinculada { get; set; }
        public virtual ItemQuestionarioParticipacaoOpcoes OpcaoSelecionada { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
        
    }
}
