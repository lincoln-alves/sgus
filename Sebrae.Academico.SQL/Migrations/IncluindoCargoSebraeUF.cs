using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.IncluindoCargoSebraeUC)]
    public class IncluindoCargoSebraeUF : Migration
    {

        public override void Up()
        {
            Execute.Sql(Scripts_3400.IncluindoCargoSebraeUF);
        }

        public override void Down()
        {
            Execute.Sql(Scripts_3400.ExcluindoCargo);
        }
    }
}
