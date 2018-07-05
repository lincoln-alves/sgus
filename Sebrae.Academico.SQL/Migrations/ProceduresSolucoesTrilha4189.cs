using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProceduresSolucoesTrilha4189)]
    public class ProceduresSolucoesTrilha4189 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_solucoes_do_desempenho_geral];");
            Execute.Procedure("SP_solucoes_do_desempenho_geral_4189");            
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_solucoes_do_desempenho_geral];");
            Execute.Procedure("SP_solucoes_do_desempenho_geral");
        }
    }
}