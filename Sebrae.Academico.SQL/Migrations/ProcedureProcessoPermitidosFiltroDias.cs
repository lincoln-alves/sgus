using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureProcessoPermitidosFiltroDias)]
    public class ProcedureProcessoPermitidosFiltroDias : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Execute.Sql("DROP PROCEDURE [dbo].[SP_PROCESSOS_PERMITIDOS_INICIAR];");
            Execute.Procedure("SP_PROCESSOS_PERMITIDOS_INICIAR");
        }
    }
}
