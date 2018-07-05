using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Collections.Generic;

namespace Sebrae.Academico.BM.Classes
{
    public class BMQuestionarioAssociacao : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<QuestionarioAssociacao> repositorio;

        public BMQuestionarioAssociacao()
        {
            repositorio = new RepositorioBase<QuestionarioAssociacao>();

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

            this.ValidarInformacoesDoFiltro(questionario, trilhaNivel);

            QuestionarioAssociacao questionarioAssociacao = null;

            var query = repositorio.session.Query<QuestionarioAssociacao>();

            query = query.Fetch(x => x.Questionario); 
            query = query.Fetch(x => x.TipoQuestionarioAssociacao);
            query = query.Fetch(x => x.TrilhaNivel);

            //Busca a Associação pelo id do questionário, pelo tipo do questionário e pelo id do nível da trilha
            questionarioAssociacao = query.FirstOrDefault(x => x.Questionario.ID == questionario.ID &&
                                                          x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao &&
                                                          x.TrilhaNivel.ID == trilhaNivel.ID &&
                                                          x.Evolutivo == evolutivo);

            return questionarioAssociacao;
        }

        public IEnumerable<QuestionarioAssociacao> ObterPorTurma(Turma turma)
        {
            return repositorio.session.Query<QuestionarioAssociacao>().Where(q => q.Turma.ID == turma.ID);
        }

        public void Salvar(QuestionarioAssociacao pQuestionarioAssociacao) {
            repositorio.Salvar(pQuestionarioAssociacao);
        }

        public void Excluir(QuestionarioAssociacao pQuestionarioAssociacao){
            repositorio.LimparSessao();
            repositorio.Excluir(pQuestionarioAssociacao);
        }

        private void ValidarInformacoesDoFiltro(Questionario questionario, TrilhaNivel trilha){
            if (questionario == null)
                throw new Exception("Questionário. Campo Obrigatório!");

            if (trilha == null)
                throw new Exception("Trilha. Campo Obrigatório!");
        }


        public IList<QuestionarioAssociacao> ObterPorFiltro(QuestionarioAssociacao pQuestionarioAssociacao)
        {
            var query = repositorio.session.Query<QuestionarioAssociacao>();

            if (pQuestionarioAssociacao != null)
            {
                if (pQuestionarioAssociacao.TrilhaNivel != null) 
                    query = query.Where(x => x.TrilhaNivel.ID == pQuestionarioAssociacao.TrilhaNivel.ID);

                if (pQuestionarioAssociacao.Questionario != null)
                    query = query.Where(x => x.Questionario.ID == pQuestionarioAssociacao.ID);
            }

            return query.ToList();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IQueryable<QuestionarioAssociacao> ObterTodos()
        {
            return repositorio.ObterTodosIQueryable();
        }
    }
}
