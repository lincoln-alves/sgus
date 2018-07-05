using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AtualizarProcedureObterLojasMapa)]
    public class AtualizarProcedureObterLojasMapa : Migration
    {
        public override void Up()
        {
            Execute.Procedure("SP_OBTER_LOJAS_MAPA");
        }

        public override void Down()
        {
        }
    }
}
