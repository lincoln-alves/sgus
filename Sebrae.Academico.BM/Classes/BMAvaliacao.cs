using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMAvaliacao : BusinessManagerBase, IDisposable
    {

        private readonly RepositorioBase<Avaliacao> _repositorioAvaliacao;
        private readonly RepositorioBase<QuestaoResposta> _repositorioQuestaoResposta;
        
        public BMAvaliacao()
        {
            _repositorioAvaliacao = new RepositorioBase<Avaliacao>();
            _repositorioQuestaoResposta = new RepositorioBase<QuestaoResposta>();
        }

        public IQueryable<Avaliacao> ObterTodos()
        {
            var query = _repositorioAvaliacao.session.Query<Avaliacao>();
            return query;
        }

        public IQueryable<Questao> ObterQuestoes()
        {
            return _repositorioAvaliacao.session.Query<Questao>();
        }

        public void Dispose()
        {
            _repositorioAvaliacao.Dispose();
            GC.Collect();
        }

        public Avaliacao Salvar(Avaliacao avaliacao)
        {
            _repositorioAvaliacao.Salvar(avaliacao);

            return avaliacao;
        }

        public QuestaoResposta Salvar(QuestaoResposta questaoResposta)
        {
            _repositorioQuestaoResposta.Salvar(questaoResposta);

            return questaoResposta;
        }

        public QuestaoResposta ObterQuestaoRespostaPorId(int questaoRespostaId)
        {
            return _repositorioQuestaoResposta.ObterPorID(questaoRespostaId);
        }

        public IQueryable<Avaliacao> ObterTodasIQueryable()
        {
            return _repositorioAvaliacao.ObterTodosIQueryable();
        }
    }
}
