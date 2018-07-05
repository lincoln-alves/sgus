using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterRespostaParticipacaoProfessorOpcoes : BusinessProcessBase
    {
        private BMRespostaParticipacaoProfessorOpcoes bmRespostaParticipacaoProfessorOpcoes;

        public ManterRespostaParticipacaoProfessorOpcoes()
            : base()
        {
            bmRespostaParticipacaoProfessorOpcoes = new BMRespostaParticipacaoProfessorOpcoes();
        }

        public void IncluirEstiloItemQuestionario(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            bmRespostaParticipacaoProfessorOpcoes.Salvar(pRespostaParticipacaoProfessorOpcoes);
        }

        public void AlterarTipoQuestionario(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            bmRespostaParticipacaoProfessorOpcoes.Salvar(pRespostaParticipacaoProfessorOpcoes);
        }

        public IList<RespostaParticipacaoProfessorOpcoes> ObterTodosRespostaParticipacaoProfessorOpcoes()
        {
            return bmRespostaParticipacaoProfessorOpcoes.ObterTodos();
        }

        public RespostaParticipacaoProfessorOpcoes ObterRespostaParticipacaoProfessorOpcoesPorID(int pId)
        {
            return bmRespostaParticipacaoProfessorOpcoes.ObterPorID(pId);
        }

        public IList<RespostaParticipacaoProfessorOpcoes> ObterRespostaParticipacaoProfessorOpcoesPorFiltro(RespostaParticipacaoProfessorOpcoes pRespostaParticipacaoProfessorOpcoes)
        {
            return bmRespostaParticipacaoProfessorOpcoes.ObterPorFiltro(pRespostaParticipacaoProfessorOpcoes);
        }

        public IQueryable<RespostaParticipacaoProfessorOpcoes> ObterTodosIQueryable()
        {
            return bmRespostaParticipacaoProfessorOpcoes.ObterTodosIQueryable();
        }

        public void Salvar(RespostaParticipacaoProfessorOpcoes rppo)
        {
            bmRespostaParticipacaoProfessorOpcoes.Salvar(rppo);
        }

        public IQueryable<RespostaParticipacaoProfessorOpcoes> ObterPorRespostaProfessor(RespostaParticipacaoProfessor rpp)
        {
            return ObterTodosIQueryable().Where(x => x.RespostaParticipacaoProfessor.ID == rpp.ID);
        }
    }
}
