using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProcedureObterLojasMapa2632)]
    public class ProcedureObterLojasMapa2632 : Migration
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
