using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlterandoProcedureDesempenhoAcademico)]
    public class AlterandoProcedureDesempenhoAcademico : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_REL_DESEMPENHO_ACADEMICO];");
            Execute.Procedure("SP_REL_DESEMPENHO_ACADEMICO");
        }
    }
}
