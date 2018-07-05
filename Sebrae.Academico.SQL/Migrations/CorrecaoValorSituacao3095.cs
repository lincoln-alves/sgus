using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CorrecaoValorSituacao3095)]
    public class CorrecaoValorSituacao3095 : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE TB_UsuarioCertificadoCertame SET VL_Situacao = '1' WHERE VL_Situacao LIKE 'Presente'");

            Alter.Column("VL_Situacao").OnTable("TB_UsuarioCertificadoCertame").AsInt32();
        } 

        public override void Down()
        {
            Alter.Column("VL_Situacao").OnTable("TB_UsuarioCertificadoCertame").AsString(200);
        }
    }
}