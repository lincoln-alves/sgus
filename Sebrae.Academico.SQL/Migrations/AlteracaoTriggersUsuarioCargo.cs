using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlteracaoTriggersUsuarioCargo)]
    public class AlteracaoTriggersUsuarioCargo : Migration
    {
        public override void Up()
        {
            Execute.Sql(Scripts_2824.TriggersUsuarioCargo);
            Delete.Column("ID_UF").FromTable("HT_UsuarioCargo");
        }

        public override void Down()
        {
            Create.Column("ID_UF").OnTable("HT_UsuarioCargo").AsInt32().Nullable();
            Execute.Sql(Scripts_2824.RollbackTriggersUsuarioCargo);
        }
    }
}