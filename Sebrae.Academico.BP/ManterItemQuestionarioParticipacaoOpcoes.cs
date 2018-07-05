using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterItemQuestionarioParticipacaoOpcoes : BusinessProcessBase
    {

        private readonly BMItemQuestionarioParticipacaoOpcoes _bmItemQuestionarioOpcoes;

        public ManterItemQuestionarioParticipacaoOpcoes()
        {
            _bmItemQuestionarioOpcoes = new BMItemQuestionarioParticipacaoOpcoes();
        }

        public ItemQuestionarioParticipacaoOpcoes ObterRespostaSelecionada(ItemQuestionarioParticipacao item)
        {
            return _bmItemQuestionarioOpcoes.ObterRespostaSelecionada(item);
        }

        public IQueryable<ItemQuestionarioParticipacaoOpcoes> ObterRespostasSelecionadas(
            List<ItemQuestionarioParticipacao> questionariosParticipacoes)
        {
            return
                _bmItemQuestionarioOpcoes.ObterRespostasSelecionadas(
                    questionariosParticipacoes.Select(x => x.ID).ToList());
        }
    }
}
