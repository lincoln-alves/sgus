using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Runtime.Serialization;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemQuestionario : EntidadeBasicaPorId, ICloneable
    {

        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ItemQuestionario()
        {
            this.ListaItemQuestionarioOpcoes = new List<ItemQuestionarioOpcoes>();
        }

        public virtual string Questao { get; set; }
        public virtual Questionario Questionario { get; set; }
        public virtual ItemQuestionario QuestionarioEnunciado { get; set; }
        public virtual TipoItemQuestionario TipoItemQuestionario { get; set; }
        public virtual EstiloItemQuestionario EstiloItemQuestionario { get; set; }
        public virtual decimal ValorQuestao { get; set; }
        public virtual string NomeGabarito { get; set; }
        public virtual IList<ItemQuestionarioOpcoes> ListaItemQuestionarioOpcoes { get; set; }
        public virtual string Feedback { get; set; }
        public virtual string Comentario { get; set; }
        public virtual int? Ordem { get; set; }
        public virtual bool InAvaliaProfessor { get; set; }
        public virtual bool? ExibeFeedback { get; set; }
        public virtual bool? RespostaObrigatoria { get; set; }

        public virtual string InAvaliaProfessorFormatado {
            get { return InAvaliaProfessor ? "Sim" : "Não"; }
        }


        public override bool Equals(object obj)
        {
            ItemQuestionario objeto = obj as ItemQuestionario;
            return objeto == null ? false : this.Questao.Equals(objeto.Questao) && this.ID.Equals(objeto.ID);
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
