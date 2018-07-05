using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.MudancaEstruturaTurma)]
    public class MudancaEstruturaTurma : Migration
    {
        public override void Up()
        {
            Create.Column("IN_AvaliacaoAprendizagem").OnTable("TB_Turma").AsBoolean().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("IN_AvaliacaoAprendizagem").FromTable("TB_Turma");
        }
    }
}