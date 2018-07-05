using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureRanking3205)]
    public class ProcedureRanking3205 : Migration
    {
        public override void Up()
        {
            Execute.Function("FN_ObterTrofeu");

            Execute.Procedure("SP_RANKING_TRILHAS");
        }

        public override void Down()
        {
        }
    }
}