using System.Security.Claims;
using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureSolucoesMochila)]
    public class ProcedureSolucoesMochila : Migration
    {
        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE SP_OBTER_SOLUCOES;");
            Execute.Sql("DROP PROCEDURE SP_OBTER_LOJAS_MAPA;");
            Execute.Procedure("SP_OBTER_SOLUCOES");
            Execute.Procedure("SP_OBTER_LOJAS_MAPA");
        }

        public override void Down()
        {
        }
    }
}