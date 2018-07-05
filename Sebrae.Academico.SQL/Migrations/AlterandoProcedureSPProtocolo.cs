using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlterandoProcedureProtocolo)]
    public class AlterandoProcedureSPProtocolo : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_Protocolo];");
            Execute.Procedure("SP_Protocolo");
        }
    }
}
