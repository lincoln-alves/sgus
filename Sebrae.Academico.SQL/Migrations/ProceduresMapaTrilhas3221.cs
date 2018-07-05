using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProceduresMapaTrilhas3221)]
    public class ProceduresMapaTrilhas3221 : Migration
    {
        public override void Up()
        {
            Execute.Procedure("SP_OBTER_LOJAS_MAPA");
            Execute.Procedure("SP_OBTER_PARTICIPANTES_MAPA");
        }

        public override void Down()
        {
        }
    }
}