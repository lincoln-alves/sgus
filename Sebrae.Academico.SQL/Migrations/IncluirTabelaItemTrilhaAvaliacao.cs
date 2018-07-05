using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.IncluirTabelaItemTrilhaAvaliacao)]
    public class IncluirTabelaItemTrilhaAvaliacao : Migration
    {
        public override void Down()
        {
            Execute.Sql("DROP TABLE dbo.TB_ItemTrilhaAvaliacao");
        }

        public override void Up()
        {
            Execute.Sql(@"CREATE TABLE [dbo].[TB_ItemTrilhaAvaliacao] (
            [ID_ItemTrilhaAvaliacao] int NOT NULL IDENTITY(1,1),
            [TX_Resenha] varchar(1024) NULL,
            [VL_Avaliacao] int NOT NULL,
            [DT_UltimaAtualizacao] datetime NOT NULL,
            [ID_ItemTrilha] int NOT NULL,
            [ID_UsuarioTrilha] int NOT NULL,
            PRIMARY KEY ([ID_ItemTrilhaAvaliacao]),
            FOREIGN KEY ([ID_ItemTrilha]) REFERENCES [dbo].[TB_ItemTrilha] ([ID_ItemTrilha]) ON DELETE NO ACTION ON UPDATE NO ACTION,
            FOREIGN KEY ([ID_UsuarioTrilha]) REFERENCES [dbo].[TB_UsuarioTrilha] ([ID_UsuarioTrilha]) ON DELETE NO ACTION ON UPDATE NO ACTION)
            GO");
        }
    }
}
