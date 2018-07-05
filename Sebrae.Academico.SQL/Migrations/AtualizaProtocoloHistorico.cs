using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{

    [Migration(NumerosMigracoes.AtualizarProtocoloHistorico)]
    public class AtualizaProtocoloHistorico : Migration
    {
        public override void Down()
        {            
        }

        public override void Up()
        {
            Execute.Sql("ALTER TABLE [dbo].[TB_Protocolo] ADD [IN_Arquivado] bit NOT NULL DEFAULT 0");
            Execute.Sql("DROP PROCEDURE [dbo].[SP_ProtocoloHistorico]");
            Execute.Procedure("SP_ProtocoloHistorico");
        }
    }
}
