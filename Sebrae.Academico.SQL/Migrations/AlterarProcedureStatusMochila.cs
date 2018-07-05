using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlterarProcedureStatusMochila)]
    public class AlterandoProcedureStatusMochila : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_MOCHILA_STATUS_MISSOES];");
            Execute.Procedure("SP_MOCHILA_STATUS_MISSOES4189");
        }
    }
}
