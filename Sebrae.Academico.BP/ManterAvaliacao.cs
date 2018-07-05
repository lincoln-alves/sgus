using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterAvaliacao : BusinessProcessBase
    {
        private readonly BMAvaliacao _bmAvaliacao;

        public ManterAvaliacao()
        {
            _bmAvaliacao = new BMAvaliacao();
        }

        public IQueryable<Questao> ObterQuestoes(bool ordenar = true)
        {
            return _bmAvaliacao.ObterQuestoes().OrderBy(x => x.Ordem);
        }

        public Avaliacao Salvar(Avaliacao avaliacao)
        {
            _bmAvaliacao.Salvar(avaliacao);

            return avaliacao;
        }

        public QuestaoResposta Salvar(QuestaoResposta questaoResposta)
        {
            return _bmAvaliacao.Salvar(questaoResposta);
        }

        public QuestaoResposta ObterQuestaoRespostaPorId(int questaoRespostaId)
        {
            return _bmAvaliacao.ObterQuestaoRespostaPorId(questaoRespostaId);
        }

        public IQueryable<Avaliacao> ObterTodasIQueryable()
        {
            return _bmAvaliacao.ObterTodasIQueryable();
        }
    }
}
