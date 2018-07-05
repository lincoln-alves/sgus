using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlterarItemTrilha4189)]
    public class AlterarItemTrilha4189 : Migration
    {
        public override void Up()
        {            
            Execute.Sql("ALTER TABLE [dbo].[TB_ItemTrilha] ADD [IN_PermiteEnvioArquivo] bit NULL DEFAULT 0");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE [dbo].[TB_ItemTrilha] DROP COLUMN IN_PermiteEnvioArquivo");            
        }
    }
}
