using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP
{
    public class ManterQuestionarioAssociacao : BusinessProcessBase
    {
        private BMQuestionarioAssociacao questionarioAssociacao = null;

        /// <summary>
        /// Método Construtor da classe
        /// </summary>
        public ManterQuestionarioAssociacao()
            : base()
        {
            questionarioAssociacao = new BMQuestionarioAssociacao();
        }

        /// <summary>
        /// Busca a Associação pelo id do questionário, pelo tipo do questionário e pelo id do nível da trilha
        /// </summary>
        /// <param name="questionario"></param>
        /// <param name="trilhaNivel"></param>
        /// <param name="tipoQuestionarioAssociacao"></param>
        /// <returns></returns>
        public QuestionarioAssociacao ObterPorFiltro(Questionario questionario, TrilhaNivel trilhaNivel,
                                                     enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao, bool evolutivo)
        {
            return questionarioAssociacao.ObterPorFiltro(questionario, trilhaNivel, tipoQuestionarioAssociacao, evolutivo);
        }

        public Questionario ObterPesquisaPosTurma(int idTurma)
        {
            // Obter associacao do questionário pós de pesquisa da turma informada.
            var associacao =
                questionarioAssociacao.ObterTodos()
                    .FirstOrDefault(
                        x =>
                            x.Turma.ID == idTurma &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                            x.Questionario.TipoQuestionario.ID == (int)enumTipoQuestionario.Pesquisa);

            // Se existir associacao, retorna o questionário informado.
            if (associacao != null)
            {
                return
                    associacao
                        .Questionario;
            }

            return null;
        }

        /// <summary>
        /// Recupera todos os questionários do tipo pesquisa/eficácia do dia atual
        /// </summary>
        /// <returns></returns>
        public IList<QuestionarioAssociacao> ObterTodosEficaciaDia()
        {
            return ObterPorIntervalo(10).ToList();
        }

        // Obter todos no intervalo determinado
        public IQueryable<QuestionarioAssociacao> ObterPorIntervalo(int intervalo)
        {
            DateTime inicio = DateTime.Now.AddMinutes(-intervalo);
            DateTime fim = DateTime.Now;

            var questionarios = questionarioAssociacao.ObterTodos();

            questionarios = questionarios.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia && x.DataDisparoLinkEficacia.HasValue &&
           x.DataDisparoLinkEficacia >= inicio && x.DataDisparoLinkEficacia <= fim);

            return questionarios;
        }
    }
}
