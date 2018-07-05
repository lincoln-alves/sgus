using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ViewQuestionarioRespostaMap : ClassMap<ViewQuestionarioResposta>
    {
        public ViewQuestionarioRespostaMap()
        {
            Table("VW_QuestionarioResposta");
            Id(x => x.ID, "ID_ItemQuestionarioParticipacaoOpcoes").GeneratedBy.Assigned();
            Map(x => x.ID_ItemQuestionarioParticipacao).Column("ID_ItemQuestionarioParticipacao");
            Map(x => x.ID_Questionario).Column("ID_Questionario");
            Map(x => x.ID_Turma).Column("ID_Turma");
            Map(x => x.NM_Opcao).Column("NM_Opcao");
        }
    }
}
