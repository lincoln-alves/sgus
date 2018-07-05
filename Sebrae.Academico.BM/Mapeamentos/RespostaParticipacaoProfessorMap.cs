using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class RespostaParticipacaoProfessorMap : ClassMap<RespostaParticipacaoProfessor>
    {
        public RespostaParticipacaoProfessorMap()
        {
            Table("TB_RespostaParticipacaoProfessor");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("id_respostaparticipacaoprofessor");
            References(x => x.ItemQuestionarioParticipacao).Column("ID_itemquestionarioparticipacao");
            References(x => x.QuestionarioParticipacao).Column("ID_questionarioparticipacao");
            References(x => x.Professor).Column("ID_professor");
            Map(x => x.Resposta).Column("nm_resposta");
            //(x => x.ListaRespostaParticipacaoOpcoes)
            //    .KeyColumn("id_respostaparticipacaoprofessor").AsBag()
            //       .Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
