using System.Security.Claims;
using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.RemoverCargoDemandanteDoProcessoResposta)]
    public class RemoverCargoDemandanteDoProcessoResposta : Migration
    {
        public override void Up()
        {
            Execute.Sql(Scripts_2839.RemoverConstaintUsuarioCargoProcessoResposta);
            Delete.Column("ID_UsuarioCargo").FromTable("TB_ProcessoResposta");
        }

        public override void Down()
        {
            Create.Column("ID_UsuarioCargo").OnTable("TB_ProcessoResposta").AsInt32().Nullable();

            Create.ForeignKey("FK_UsuarioCargo_ProcessoResposta")
                .FromTable("TB_ProcessoResposta")
                .ForeignColumn("ID_UsuarioCargo")
                .ToTable("TB_UsuarioCargo")
                .PrimaryColumn("ID_UsuarioCargo");
        }
    }
}