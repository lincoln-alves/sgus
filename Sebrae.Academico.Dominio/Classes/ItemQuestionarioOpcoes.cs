using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    /// <summary>
    /// Classe de Domínio que representa uma resposta.
    /// </summary>
    public class ItemQuestionarioOpcoes : EntidadeBasica
    {

        public virtual ItemQuestionario ItemQuestionario { get; set; }
        public virtual bool RespostaCorreta { get; set; }
        public virtual enumTipoDiagnostico TipoDiagnostico { get; set; }
        public virtual byte? OpcaoInt { get; set; }
        public virtual ItemQuestionarioOpcoes OpcaoVinculada { get; set; }

        public override bool Equals(object obj)
        {
            ItemQuestionarioOpcoes objeto = obj as ItemQuestionarioOpcoes;
            return objeto == null ? false : this.ID.Equals(objeto.ID);
        }
    }
}
