using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.FunctionMoedasConquistadas4190_2)]
    public class FunctionMoedasConquistadas4190 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DROP FUNCTION FN_ObterMoedasConquistadasPontoSebrae");
            Execute.Function("FN_ObterMoedasConquistadasPontoSebrae");
        }

        public override void Down()
        {
        }
    }
}