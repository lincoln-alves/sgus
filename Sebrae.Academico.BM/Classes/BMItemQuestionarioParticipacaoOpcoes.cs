using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMItemQuestionarioParticipacaoOpcoes : BusinessManagerBase
    {
        private readonly RepositorioBase<ItemQuestionarioOpcoes> _repositorio;

        public BMItemQuestionarioParticipacaoOpcoes()
        {
            _repositorio = new RepositorioBase<ItemQuestionarioOpcoes>();
        }

        public ItemQuestionarioParticipacaoOpcoes ObterRespostaSelecionada(ItemQuestionarioParticipacao item)
        {
            return _repositorio.session.Query<ItemQuestionarioParticipacaoOpcoes>()
                .FirstOrDefault(x => x.ItemQuestionarioParticipacao.ID == item.ID && x.RespostaSelecionada == true);
        }

        public IQueryable<ItemQuestionarioParticipacaoOpcoes> ObterRespostasSelecionadas(
            List<int> idsQuestionariosParticipacoes)
        {
            return _repositorio.session.Query<ItemQuestionarioParticipacaoOpcoes>()
                .Where(
                    x =>
                        idsQuestionariosParticipacoes.Contains(x.ItemQuestionarioParticipacao.ID) &&
                        x.RespostaSelecionada == true);
        }
    }
}