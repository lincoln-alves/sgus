using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProtocoloArquivado)]
    public class ProtocoloArquivado : Migration
    {
        public override void Down()
        {
            Delete.Column("IN_Arquivado").FromTable("TB_Protocolo");
        }

        public override void Up()
        {
            Create.Column("IN_Arquivado").OnTable("TB_Protocolo").AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}
