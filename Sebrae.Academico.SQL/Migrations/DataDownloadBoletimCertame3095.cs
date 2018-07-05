using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.DataDownloadBoletimCertame3095)]
    public class DataDownloadBoletimCertame3095 : Migration
    {
        public override void Up()
        {
            Create.Column("DT_DataDownloadBoletim").OnTable("TB_UsuarioCertificadoCertame").AsDateTime().Nullable();
            Create.Column("DT_DataDownloadBoletim").OnTable("HT_UsuarioCertificadoCertame").AsDateTime().Nullable();

            Execute.Sql(Scripts_3095.CommitTriggersUsuarioCertame);
        }

        public override void Down()
        {
            Execute.Sql(Scripts_3095.RollbackTriggersUsuarioCertame);

            Delete.Column("DT_DataDownloadBoletim").FromTable("TB_UsuarioCertificadoCertame");
            Delete.Column("DT_DataDownloadBoletim").FromTable("HT_UsuarioCertificadoCertame");
        }
    }
}