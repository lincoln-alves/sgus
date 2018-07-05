using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureQuestionariosPendentes3578)]
    public class ProcedureQuestionariosPendentes3578 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_TURMAS_ABANDONO_PENDENTES];");
            Execute.Procedure("SP_TURMAS_ABANDONO_PENDENTES");
        }

        public override void Down()
        {
        }
    }
}