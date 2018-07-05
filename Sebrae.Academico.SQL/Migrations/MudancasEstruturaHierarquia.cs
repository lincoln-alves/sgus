using System.Security.Claims;
using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.MudancasEstruturaHierarquia)]
    public class MudancasEstruturaHierarquia : Migration
    {
        public override void Up()
        {
            Create.Column("ID_Cargo_New").OnTable("TB_Cargo").AsInt32().NotNullable().Identity();
            Create.Column("ID_CargoPai_New").OnTable("TB_Cargo").AsInt32().Nullable();
            Create.Column("IN_Ativo").OnTable("TB_Cargo").AsBoolean().WithDefaultValue(true);
            Create.Column("VL_Order").OnTable("TB_Cargo").AsInt32().Nullable();
            Create.Column("VL_Sigla").OnTable("TB_Cargo").AsString(100).Nullable();

            Execute.Sql(Scripts_2824.RemoverConstaintUsuarioCargo);
            Execute.Sql(Scripts_2824.RecriarIds);
            Execute.Sql(Scripts_2824.RemoverConstraintCargoPai);
            Execute.Sql(Scripts_2824.RemoverConstraintCargo);

            // Remover colunas velhas do Cargo.
            Delete.Column("ID_CargoPai").FromTable("TB_Cargo");
            Delete.Column("ID_Cargo").FromTable("TB_Cargo");

            Rename.Column("ID_Cargo_New").OnTable("TB_Cargo").To("ID_Cargo");
            Rename.Column("ID_CargoPai_New").OnTable("TB_Cargo").To("ID_CargoPai");
            
            // Readicionar a chave primária do Cargo.
            Execute.Sql("ALTER TABLE TB_Cargo ADD CONSTRAINT PK_TB_Cargo PRIMARY KEY(ID_Cargo)");

            // Recriar relação do Cargo com Cargo pai.
            Create.ForeignKey("FK_Cargo_CargoPai")
                .FromTable("TB_Cargo")
                .ForeignColumn("ID_CargoPai")
                .ToTable("TB_Cargo")
                .PrimaryColumn("ID_Cargo");

            // Recriar relação do Cargo com Usuário Cargo.
            Create.ForeignKey("FK_Cargo_UsuarioCargo")
                .FromTable("TB_UsuarioCargo")
                .ForeignColumn("ID_Cargo")
                .ToTable("TB_Cargo")
                .PrimaryColumn("ID_Cargo");

            // Criar relação do Cargo com UF.
            Create.Column("ID_UF").OnTable("TB_Cargo").AsCustom("TINYINT").NotNullable().WithDefaultValue(1);

            Create.ForeignKey("FK_Cargo_UF")
                .FromTable("TB_Cargo")
                .ForeignColumn("ID_UF")
                .ToTable("TB_UF")
                .PrimaryColumn("ID_UF");    

            // Criar relação da EtapaResposta com o Usuario Cargo
            Create.Column("ID_Cargo").OnTable("TB_EtapaResposta").AsInt32().Nullable();

            Execute.Sql(Scripts_2824.MigrarUsuarioCargoEmEtapaResposta);

            Create.ForeignKey("FK_EtapaResposta_Cargo")
                .FromTable("TB_EtapaResposta")
                .ForeignColumn("ID_Cargo")
                .ToTable("TB_Cargo")
                .PrimaryColumn("ID_Cargo");

            Create.Column("VL_Order").OnTable("HT_Cargo").AsInt32().Nullable();
            Create.Column("IN_Ativo").OnTable("HT_Cargo").AsBoolean().WithDefaultValue(true);
            Create.Column("VL_Sigla").OnTable("HT_Cargo").AsString(100).Nullable();
            Execute.Sql(Scripts_2824.TriggersCargo);

            // Remover UF do UsuarioCargo
            Execute.Sql(Scripts_2824.RemoverConstraintUsuarioCargoUf);
            Delete.Index("Usuario_Cargo_Uf").OnTable("TB_UsuarioCargo");
            Delete.Column("ID_UF").FromTable("TB_UsuarioCargo");
            Create.Index("Usuario_Cargo").OnTable("TB_UsuarioCargo")
                .OnColumn("ID_Usuario").Ascending()
                .OnColumn("ID_Cargo").Ascending();
        }

        public override void Down()
        {
            Delete.Index("Usuario_Cargo").OnTable("TB_UsuarioCargo");
            Create.Column("ID_UF").OnTable("TB_UsuarioCargo").AsCustom("TINYINT").NotNullable().WithDefaultValue(1);
            Create.ForeignKey("FK_UsuarioCargo_Uf")
                .FromTable("TB_UsuarioCargo")
                .ForeignColumn("ID_UF")
                .ToTable("TB_UF")
                .PrimaryColumn("ID_UF");
            Create.Index("Usuario_Cargo_Uf").OnTable("TB_UsuarioCargo")
                .OnColumn("ID_Usuario").Ascending()
                .OnColumn("ID_Cargo").Ascending()
                .OnColumn("ID_UF").Ascending();

            Execute.Sql(Scripts_2824.RecriarTabelaSemIdentity);

            Delete.ForeignKey("FK_Cargo_UF").OnTable("TB_Cargo");
            Delete.Column("ID_UF").FromTable("TB_Cargo");

            Delete.ForeignKey("FK_EtapaResposta_Cargo").OnTable("TB_EtapaResposta");
            Delete.Column("ID_Cargo").FromTable("TB_EtapaResposta");
            
            Delete.Column("IN_Ativo").FromTable("TB_Cargo");
            Delete.Column("VL_Order").FromTable("TB_Cargo");
            Delete.Column("VL_Sigla").FromTable("TB_Cargo");

            Delete.Column("IN_Ativo").FromTable("HT_Cargo");
            Delete.Column("VL_Order").FromTable("HT_Cargo");
            Delete.Column("VL_Sigla").FromTable("HT_Cargo");
            Execute.Sql(Scripts_2824.RollbackTriggersCargo);
        }
    }
}