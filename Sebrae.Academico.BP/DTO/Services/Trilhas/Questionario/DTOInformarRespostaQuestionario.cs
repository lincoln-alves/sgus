using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.Questionario
{
    public class DTOInformarRespostaQuestionario
    {
        public int Id { get; set; }

        public List<DTOQuestao> Questoes { get; set; }

        public bool IsAprovado(Academico.Dominio.Classes.Questionario questionario, decimal? notaMinima = null)
        {
            var nota = ObterNota(questionario);
            return nota >= (notaMinima ?? questionario.NotaMinima);
        }

        /// <summary>
        /// Obter a nota simulada do usuário no questionário informado.
        /// Compara os dados informados com os dados cadastrados.
        /// </summary>
        /// <returns></returns>
        public decimal ObterNota(Academico.Dominio.Classes.Questionario questionario)
        {
            decimal somaValorRespostasCertas = 0;
            decimal somaValorRespostasErradas = 0;

            questionario = questionario ?? new ManterQuestionario().ObterQuestionarioPorID(Id);

            if (questionario == null)
                return 0;

            foreach (var itemQuestionarioDto in Questoes)
            {
                var itemQuestionario = questionario.ListaItemQuestionario.FirstOrDefault(x => x.ID == itemQuestionarioDto.Id);

                if (itemQuestionario == null)
                    continue;

                var valorQuestao = itemQuestionario.ValorQuestao;

                // Para provas, só interessam as questões objetivas.
                if (itemQuestionario.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.Discursiva ||
                    itemQuestionario.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes)
                    continue;

                switch ((enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID)
                {
                    case enumTipoItemQuestionario.Objetiva:
                    case enumTipoItemQuestionario.MultiplaEscolha:
                    case enumTipoItemQuestionario.VerdadeiroOuFalso:
                        var respostaSelecionada =
                            itemQuestionario.ListaItemQuestionarioOpcoes.FirstOrDefault(
                                x => x.ID == itemQuestionarioDto.RespostaSelecionada);

                        if (respostaSelecionada == null)
                            continue;

                        // Se a resposta selecionada for a correta, pontua. Se não, pontua o erro.
                        if (respostaSelecionada.RespostaCorreta)
                            somaValorRespostasCertas += valorQuestao;
                        else
                            somaValorRespostasErradas += valorQuestao;

                        break;
                    case enumTipoItemQuestionario.ColunasRelacionadas:
                        foreach (var opcao in itemQuestionario.ListaItemQuestionarioOpcoes.Where(x => x.OpcaoVinculada != null))
                        {
                            // Aqui que é o grande pulo do gato.
                            // Se o ID da opção vinculada for igual ao ID da opção, a resposta está correta.
                            if (itemQuestionarioDto.Opcoes.Any(o => o.Id == opcao.OpcaoVinculada.ID))
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

            // Assumindo que a nota máxima dos questionários seja 10, 
            return (10 * somaValorRespostasCertas) / somaTotalValoresQuestoes;
        }

        public dynamic ObterGabaritoComFeedback(Academico.Dominio.Classes.Questionario questionario = null)
        {
            questionario = questionario ?? new ManterQuestionario().ObterQuestionarioPorID(Id);

            var listaRetorno = new List<dynamic>();

            foreach (var itemQuestionarioDto in Questoes)
            {
                dynamic retorno = new ExpandoObject();

                var itemQuestionario = questionario.ListaItemQuestionario.FirstOrDefault(x => x.ID == itemQuestionarioDto.Id);

                if (itemQuestionario == null)
                    continue;

                retorno.Questao = itemQuestionario.Questao;

                dynamic retornoQuestao = new ExpandoObject();

                var respostasSelecionadas = new List<dynamic>();
                var respostasCorretas = new List<dynamic>();

                switch ((enumTipoItemQuestionario)itemQuestionario.TipoItemQuestionario.ID)
                {
                    case enumTipoItemQuestionario.Objetiva:
                    case enumTipoItemQuestionario.VerdadeiroOuFalso:
                        var respostaSelecionada =
                           itemQuestionario.ListaItemQuestionarioOpcoes.FirstOrDefault(
                               x => x.ID == itemQuestionarioDto.RespostaSelecionada);

                        if (respostaSelecionada == null)
                            continue;


                        respostasSelecionadas.Add(new
                        {
                            Texto = respostaSelecionada.Nome,
                            IsCorreto = respostaSelecionada.RespostaCorreta
                        });

                        if (respostaSelecionada.RespostaCorreta != true)
                        {
                            var respostaCorreta = itemQuestionario.ListaItemQuestionarioOpcoes.FirstOrDefault(
                                x => x.RespostaCorreta);

                            if (respostaCorreta != null)
                                respostasCorretas.Add(respostaCorreta.Nome);
                        }

                        break;
                    case enumTipoItemQuestionario.MultiplaEscolha:
                        foreach (var opcao in itemQuestionario.ListaItemQuestionarioOpcoes)
                        {
                            var opcaoDto = itemQuestionarioDto.Opcoes.FirstOrDefault(x => x.Id == opcao.ID);

                            if (opcaoDto != null)
                            {
                                respostasSelecionadas.Add(new
                                {
                                    Texto = opcao.Nome,
                                    IsCorreto = opcao.RespostaCorreta
                                });
                            }
                            else
                            {
                                if (opcao.RespostaCorreta)
                                    respostasCorretas.Add(opcao.Nome);
                            }
                        }

                        break;
                    case enumTipoItemQuestionario.ColunasRelacionadas:
                        foreach (var opcao in itemQuestionario.ListaItemQuestionarioOpcoes.Where(x => x.OpcaoVinculada != null))
                        {
                            var opcaoDto = itemQuestionarioDto.Opcoes.FirstOrDefault(x => x.Id == opcao.ID);

                            var opcaoSelecionada =
                                opcaoDto != null ? itemQuestionario.ListaItemQuestionarioOpcoes.FirstOrDefault(x => x.ID == opcaoDto.Valor) : null;

                            respostasSelecionadas.Add(new
                            {
                                Texto = opcao.Nome,
                                IsCorreto =
                                    opcaoDto != null && opcao.OpcaoVinculada != null && opcao.OpcaoVinculada.ID == opcaoDto.Id,
                                OpcaoSelecionada = opcaoSelecionada != null ? opcaoSelecionada.Nome : null,
                                RespostaCorreta =
                                    opcaoSelecionada != null && opcao.OpcaoVinculada != null && opcao.OpcaoVinculada.ID == opcaoSelecionada.ID
                                        ? opcao.OpcaoVinculada.Nome
                                        : null
                            });
                        }

                        break;
                    case enumTipoItemQuestionario.Discursiva:
                    case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                    case enumTipoItemQuestionario.Diagnostico:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                retornoQuestao.TipoQuestao = itemQuestionario.TipoItemQuestionario.ID;
                retornoQuestao.Feedback = itemQuestionario.ExibeFeedback == true ? itemQuestionario.Feedback : null;
                retornoQuestao.RespostasSelecionadas = respostasSelecionadas;
                retornoQuestao.RespostasCorretas = respostasCorretas;

                retorno.Resposta = retornoQuestao;

                listaRetorno.Add(retorno);
            }

            return listaRetorno;
        }
    }

    public class DTOQuestao
    {
        public int Id { get; set; }
        public string Resposta { get; set; }
        public int RespostaSelecionada { get; set; }
        public List<DTOOpcaoMultipla> RespostasSelecionadas { get; set; }
        public int Tipo { get; set; }

        public List<DTOOpcao> Opcoes { get; set; }
    }

    public class DTOOpcao
    {
        public int Id { get; set; }
        public int Valor { get; set; }
    }

    public class DTOOpcaoMultipla
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
    }
}
