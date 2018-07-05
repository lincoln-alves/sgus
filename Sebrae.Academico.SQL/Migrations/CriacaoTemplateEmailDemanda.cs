using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CriacaoTemplateEmailDemanda)]
    public class CriacaoTemplateEmailDemanda : Migration
    {

        public override void Up()
        {
            Execute.Sql(Scripts_3703.IncluiTemplateEmail);
        }

        public override void Down()
        {
            Execute.Sql(Scripts_3703.ExcluiTemplateEmail);
        }
    }
}
