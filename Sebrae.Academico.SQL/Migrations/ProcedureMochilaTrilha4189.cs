using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureMochilaTrilha4189)]
    public class ProcedureMochilaTrilha4189 : Migration
    {
        public override void Up()
        {            
            Execute.Function("FN_ObterMoedasConquistadasNivelSebrae");
            //Execute.Sql("IF OBJECT_ID('SP_DADOS_MOCHILA', 'P') IS NOT NULL DROP PROCEDURE [dbo].[SP_DADOS_MOCHILA]");
            Execute.Sql("DROP PROCEDURE [dbo].[SP_DADOS_MOCHILA]");
            Execute.Procedure("SP_DADOS_MOCHILA");
        }

        public override void Down()
        {            
        }
    }
}
