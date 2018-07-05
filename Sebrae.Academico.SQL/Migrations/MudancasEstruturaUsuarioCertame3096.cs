using System.Security.Claims;
using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.MudancaEstruturaUsuarioCertame3096)]
    public class MudancaEstruturaUsuarioCertame3096 : Migration
    {
        public override void Up()
        {
            Create.Column("VL_ArquivoBoletim").OnTable("TB_UsuarioCertificadoCertame").AsString(200).Nullable();
            Create.Column("VL_ArquivoBoletim").OnTable("HT_UsuarioCertificadoCertame").AsString(200).Nullable();

            Create.Column("VL_Situacao").OnTable("TB_UsuarioCertificadoCertame").AsString(200).Nullable();
            Create.Column("VL_Situacao").OnTable("HT_UsuarioCertificadoCertame").AsString(200).Nullable();

            Create.Column("TX_Justificativa").OnTable("TB_UsuarioCertificadoCertame").AsCustom("NVARCHAR(MAX)").Nullable();
            Create.Column("TX_Justificativa").OnTable("HT_UsuarioCertificadoCertame").AsCustom("NVARCHAR(MAX)").Nullable();

            Execute.Sql(Scripts_3096.CommitTriggersUsuarioCertame);
            Execute.Sql(Scripts_3096.AtualizaTabelaUsuarioCertame);
            Execute.Sql(Scripts_3096.AtualizaTabelaUsuarioCertameSituacao);
        }

        public override void Down()
        {
            Execute.Sql(Scripts_3096.RollbackTriggersUsuarioCertame);

            Delete.Column("VL_ArquivoBoletim").FromTable("TB_UsuarioCertificadoCertame");
            Delete.Column("VL_ArquivoBoletim").FromTable("HT_UsuarioCertificadoCertame");

            Delete.Column("VL_Situacao").FromTable("TB_UsuarioCertificadoCertame");
            Delete.Column("VL_Situacao").FromTable("HT_UsuarioCertificadoCertame");

            Delete.Column("TX_Justificativa").FromTable("TB_UsuarioCertificadoCertame");
            Delete.Column("TX_Justificativa").FromTable("HT_UsuarioCertificadoCertame");
        }
    }
}