using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.MudancasProceduresDashboard2868)]
    public class MudancasProceduresDashboard2868 : Migration
    {
        public override void Up()
        {
            Execute.Procedure("DASHBOARD_REL_CONCLUINTES_EMPREGADOS");
        }

        public override void Down()
        {
        }
    }
}