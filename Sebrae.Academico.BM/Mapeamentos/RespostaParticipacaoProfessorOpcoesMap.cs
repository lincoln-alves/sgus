using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class RespostaParticipacaoProfessorOpcoesMap : ClassMap<RespostaParticipacaoProfessorOpcoes>
    {
        public RespostaParticipacaoProfessorOpcoesMap()
        {
            Table("TB_RespostaParticipacaoProfessorOpcoes");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id_respostaparticipacaoprofessoropcoes");
            References(x => x.ItemQuestionarioParticipacao).Column("ID_ItemQuestionarioParticipacao").Cascade.All();
            References(x => x.RespostaParticipacaoProfessor).Column("ID_respostaparticipacaoprofessor").Cascade.All();
            References(x => x.ItemQuestionarioParticipacaoOpcoes).Column("id_itemquestionarioparticipacaoopcoes");
            Map(x => x.RespostaSelecionada).Column("IN_RespostaSelecionada");
        }
    }
}
