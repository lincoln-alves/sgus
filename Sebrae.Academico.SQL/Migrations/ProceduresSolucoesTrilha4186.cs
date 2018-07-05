using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProceduresSolucoesTrilha)]
    public class ProceduresSolucoesTrilha4186 : Migration
    {
        public override void Up()
        {
            Execute.Procedure("SP_solucoes_do_trilheiro");
            Execute.Procedure("SP_solucoes_da_trilha");
            Execute.Procedure("SP_solucoes_do_desempenho_geral");
            Execute.Procedure("SP_cursos_online_ucsebrae");
        }

        public override void Down()
        {
        }
    }
}