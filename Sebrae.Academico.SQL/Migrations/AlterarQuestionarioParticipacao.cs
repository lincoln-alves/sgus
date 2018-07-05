using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlterarQuestionarioParticipacao)]
    public class AlterarQuestionarioParticipacao : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE TB_QuestionarioParticipacao ADD ID_ItemTrilha INT NULL;");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE TB_QuestionarioParticipacao DROP COLUMN ID_ItemTrilha;");
        }
    }
}
