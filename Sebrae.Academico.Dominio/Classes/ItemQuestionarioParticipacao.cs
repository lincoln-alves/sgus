using System.Collections.Generic;
using System;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemQuestionarioParticipacao : EntidadeBasicaPorId, ICloneable
    {
      
        public virtual QuestionarioParticipacao QuestionarioParticipacao { get; set; }
        public virtual TipoItemQuestionario TipoItemQuestionario { get; set; }
        public virtual EstiloItemQuestionario EstiloItemQuestionario { get; set; }
        public virtual string Questao { get; set; }
        public virtual decimal? ValorQuestao { get; set; }
        public virtual decimal? ValorAtribuido { get; set; }
        public virtual string Gabarito { get; set; }
        public virtual string Resposta { get; set; }
        public virtual string Feedback { get; set; }
        public virtual string Comentario { get; set; }
        public virtual int? Ordem { get; set; }
        public virtual bool InAvaliaProfessor { get; set; }
        public virtual bool? ExibeFeedback { get; set; }
        public virtual bool? RespostaObrigatoria { get; set; }

        public virtual string InAvaliaProfessorFormatado
        {
            get { return InAvaliaProfessor ? "Sim" : "Não"; }
        }

        public virtual IList<ItemQuestionarioParticipacaoOpcoes> ListaOpcoesParticipacao { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        private int? _Peso { get; set; }

        /// <summary>
        /// Retornar o peso da questão de acordo com o tipo.
        /// </summary>
        /// <returns></returns>
        public virtual int ObterPeso()
        {
            // Para evitar de ficar calculando várias vezes, mantém o valor no objeto após ser obtido uma vez.
            if (_Peso != null)
                return _Peso.Value;

            int peso;

            switch ((enumTipoItemQuestionario) TipoItemQuestionario.ID)
            {
                case enumTipoItemQuestionario.Discursiva:
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                case enumTipoItemQuestionario.Diagnostico:
                    peso = 0;
                    break;
                case enumTipoItemQuestionario.Objetiva:
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    peso = 1;
                    break;
                case enumTipoItemQuestionario.MultiplaEscolha:
                    peso = ListaOpcoesParticipacao.Count(x => x.RespostaCorreta == true);
                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    peso = ListaOpcoesParticipacao.Count();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _Peso = peso;

            return peso;
        }
    }
}
