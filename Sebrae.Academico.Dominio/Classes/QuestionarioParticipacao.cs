using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class QuestionarioParticipacao : EntidadeBasicaPorId, ICloneable
    {
        public virtual Usuario Usuario { get; set; }
        public virtual Questionario Questionario { get; set; }
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual TurmaCapacitacao TurmaCapacitacao { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime? DataParticipacao { get; set; }
        public virtual DateTime? DataLimiteParticipacao { get; set; }
        public virtual TipoQuestionarioAssociacao TipoQuestionarioAssociacao { get; set; }
        public virtual decimal ValorProva { get; set; }
        public virtual string TextoEnunciadoPre { get; set; }
        public virtual string TextoEnunciadoPos { get; set; }
        public virtual int? NotaMinima { get; set; }
        public virtual int? IdItemTrilha { get; set; }
        public virtual MatriculaTurma MatriculaTurma { get; set; }

        public virtual bool Evolutivo { get; set; }

        public virtual IList<ItemQuestionarioParticipacao> ListaItemQuestionarioParticipacao { get; set; }
        public virtual IList<ItemTrilhaParticipacao> ListaItemTrilhaParticipacao { get; set; }

        public QuestionarioParticipacao()
        {
            this.Auditoria = new Auditoria();
            ListaItemTrilhaParticipacao = new List<ItemTrilhaParticipacao>();
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        private decimal? _Nota { get; set; }

        /// <summary>
        /// Obter a nota do questionário proporcional aos valores das perguntas.
        /// </summary>
        /// <returns></returns>
        public virtual decimal ObterNota()
        {
            VerificarPermissaoProvaAtividadeTrilha();

            // Se o resultado já existir no objeto, retorna.
            if (_Nota != null)
                return _Nota.Value;

            decimal somaValorRespostasCertas = 0;
            decimal somaValorRespostasErradas = 0;

            foreach (var itemQuestionarioParticipacao in ListaItemQuestionarioParticipacao)
            {
                // Para provas, só interessam as questões objetivas.
                if (itemQuestionarioParticipacao.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.Discursiva ||
                    itemQuestionarioParticipacao.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.AgrupadorDeQuestoes)
                    continue;

                var valorQuestao = itemQuestionarioParticipacao.ValorQuestao ?? 0;

                switch ((enumTipoItemQuestionario)itemQuestionarioParticipacao.TipoItemQuestionario.ID)
                {
                    case enumTipoItemQuestionario.Objetiva:
                    case enumTipoItemQuestionario.MultiplaEscolha:
                    case enumTipoItemQuestionario.VerdadeiroOuFalso:
                        var respostaSelecionada =
                        itemQuestionarioParticipacao.ListaOpcoesParticipacao.FirstOrDefault(x => x.RespostaSelecionada == true);

                        if (respostaSelecionada == null)
                            continue;

                        // Se a resposta selecionada for a correta, pontua. Se não, pontua o erro.
                        if (respostaSelecionada.RespostaCorreta == true)
                            somaValorRespostasCertas += valorQuestao;
                        else
                            somaValorRespostasErradas += valorQuestao;

                        break;
                    case enumTipoItemQuestionario.ColunasRelacionadas:

                        foreach (
                            var opcao in
                                itemQuestionarioParticipacao.ListaOpcoesParticipacao.Where(x => x.OpcaoVinculada != null)
                            )
                        {
                            // Aqui que é o grande pulo do gato.
                            // Se o ID da opção vinculada for igual ao ID da opção, a resposta está correta.
                            if (opcao.OpcaoSelecionada != null && opcao.OpcaoVinculada != null &&
                                opcao.OpcaoVinculada.ID == opcao.OpcaoSelecionada.ID)
                            {
                                somaValorRespostasCertas += valorQuestao;
                            }
                            else
                            {
                                somaValorRespostasErradas += valorQuestao;
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var somaTotalValoresQuestoes = somaValorRespostasCertas + somaValorRespostasErradas;

            if (somaTotalValoresQuestoes == 0)
                return 0;

            // Assumindo que a nota máxima das provas seja 10.
            var nota = Math.Round(((10 * somaValorRespostasCertas) / somaTotalValoresQuestoes), 2);

            _Nota = nota;

            return nota;
        }

        public virtual bool IsAprovado(decimal? nota = null)
        {
            VerificarPermissaoProvaAtividadeTrilha();

            var notaMinima = (enumTipoQuestionario) Questionario.TipoQuestionario.ID ==
                             enumTipoQuestionario.AvaliacaoProva
                ? TrilhaNivel.NotaMinima
                : Questionario.NotaMinima;

            if (notaMinima == null)
                return true;

            return (nota ?? ObterNota()) >= notaMinima;
        }

        private void VerificarPermissaoProvaAtividadeTrilha()
        {
            var tipoQuestionario = (enumTipoQuestionario) Questionario.TipoQuestionario.ID;

            // Só permitir que este método seja utilizado por questionários de Prova e questionários de Atividade Trilha.
            if (tipoQuestionario != enumTipoQuestionario.AtividadeTrilha &&
                tipoQuestionario != enumTipoQuestionario.AvaliacaoProva)
            {
                throw new InvalidOperationException("Este método não pode ser utilizado para outros tipos de questionários.");
            }
        }
    }
}