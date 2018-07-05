using System;
using System.Data;

namespace Sebrae.Academico.Dominio.Classes
{
    public class QuestionarioAssociacao : EntidadeBasica, ICloneable
    {

        public virtual Questionario Questionario { get; set; }
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual TurmaCapacitacao TurmaCapacitacao { get; set; }
        public virtual TipoQuestionarioAssociacao TipoQuestionarioAssociacao { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual bool Evolutivo { get; set; }
        public virtual DateTime? DataDisparoLinkPesquisa { get; set; }
        public virtual DateTime? DataDisparoLinkEficacia { get; set; }


        public virtual bool IsRespondido(Usuario usuario)
        {
            return Questionario.IsRespondido(usuario, Turma);
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #region "Atributos Lógicos"

        public virtual bool CompararPorTipoItemQuestionario
        {
            get;
            set;
        }

        #endregion

        public override bool Equals(object obj)
        {
            QuestionarioAssociacao objeto = obj as QuestionarioAssociacao;

            if (objeto.CompararPorTipoItemQuestionario)
            {
                //return objeto == null ? false : this.IdLogico.Equals(objeto.IdLogico);
                return objeto == null ? false : (this.ID.Equals(objeto.ID) && (this.TipoQuestionarioAssociacao.ID.Equals(objeto.TipoQuestionarioAssociacao.ID)));
            }
            else
            {
                return objeto == null ? false : this.ID.Equals(objeto.ID);
            }
        }


        //public override bool Equals(object obj)
        //{
        //    QuestionarioAssociacao objeto = obj as QuestionarioAssociacao;
        //    return objeto == null ? false : (this.ID.Equals(objeto.ID) && (this.TipoQuestionarioAssociacao.ID.Equals(objeto.TipoQuestionarioAssociacao.ID)));
        //}

        #endregion
    }
}
