using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.NumeroInscricaoBoletimCertame3096)]
    public class NumeroInscricaoBoletimCertame3096 : Migration
    {
        public override void Up()
        {
            Create.Column("VL_Inscricao").OnTable("TB_UsuarioCertificadoCertame").AsString(100).Nullable();
            Create.Column("VL_Inscricao").OnTable("HT_UsuarioCertificadoCertame").AsString(100).Nullable();

            Execute.Sql(Scripts_3096.CommitTriggersUsuarioCertame2);

            Execute.Sql(Scripts_3096.AtualizaNumeroInscricaoUsuarioCertame);
        }

        public override void Down()
        {
            Delete.Column("VL_Inscricao").FromTable("TB_UsuarioCertificadoCertame");
            Delete.Column("VL_Inscricao").FromTable("HT_UsuarioCertificadoCertame");

            Execute.Sql(Scripts_3096.RollbackTriggersUsuarioCertame2);
        }
    }
}