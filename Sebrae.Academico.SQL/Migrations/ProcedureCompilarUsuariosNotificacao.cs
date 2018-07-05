using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureCompilarUsuariosNotificacao)]
    public class ProcedureCompilarUsuariosNotificacao : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_COMPILAR_USUARIOS_NOTIFICACAO];");
            Execute.Procedure("SP_COMPILAR_USUARIOS_NOTIFICACAO");
        }
    }
}