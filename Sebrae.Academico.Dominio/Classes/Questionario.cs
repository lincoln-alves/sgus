using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Questionario : EntidadeBasica, ICloneable
    {

        public Questionario()
        {
            ListaItemQuestionario = new List<ItemQuestionario>();
            ListaQuestionarioAssociacao = new List<QuestionarioAssociacao>();
            ListaQuestionarioParticipacao = new List<QuestionarioParticipacao>();
            ListaQuestionarioPermissao = new List<QuestionarioPermissao>();
        }

        public virtual TipoQuestionario TipoQuestionario { get; set; }
        public virtual IList<ItemQuestionario> ListaItemQuestionario { get; set; }
        public virtual IList<QuestionarioAssociacao> ListaQuestionarioAssociacao { get; set; }
        public virtual IList<QuestionarioParticipacao> ListaQuestionarioParticipacao { get; set; }
        public virtual int? PrazoMinutos { get; set; }
        public virtual int? QtdQuestoesProva { get; set; }
        public virtual int? NotaMinima { get; set; }
        public virtual IList<QuestionarioPermissao> ListaQuestionarioPermissao { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual IList<CategoriaConteudo> ListaCategoriaConteudo { get; set; }
        public virtual string TextoEnunciado { get; set; }
        public virtual IList<Campo> ListaCampos { get; set; }
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
        public virtual bool? Ativo { get; set; }

        /// <summary>
        /// Verifica se um determinado usuário possui um clone do questionário relacionado a uma determinada turma, cujo clone
        /// possua data de participação preenchida, ou seja, o questionário foi respondido.
        /// Obs.: Geralmente, os questionários que são relacionados com as turmas são Pré, Pós, Cancelamento e Reprovação.
        /// </summary>
        /// <param name="usuario">Usuário que respondeu o questionário.</param>
        /// <param name="turma">Turma vinculada ao questionário.</param>
        /// <returns>True: usuário respondeu o questionário. False: usuário ainda não clonou o questionário, ou clonou e não respondeu.</returns>
        public virtual bool IsRespondido(Usuario usuario, Turma turma)
        {
            var retorno = ListaQuestionarioParticipacao.Any(
                qp =>
                    qp.Usuario.ID == usuario.ID && qp.Turma != null && qp.Turma.ID == turma.ID &&
                    qp.DataParticipacao.HasValue);

            return retorno;
        }

        /// <summary>
        /// Verifica se o questionário pode ser duplicado ou não
        /// </summary>
        /// <param name="questionario"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public virtual bool TratarEdicaoQuestionario(Usuario usuario)
        {
            if (usuario.IsGestor() && !usuario.IsAdministrador() && Uf != null)
            {
                return usuario.UF.ID == Uf.ID;
            }

            return true;
        }


        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual Questionario NovoQuestionario (Questionario questionario)
        {
            var questionarioClonado = new Questionario();
            questionarioClonado.Nome = questionario.Nome;
            questionarioClonado.TipoQuestionario = questionario.TipoQuestionario;
            return questionario;
        }

        #endregion

    }
}