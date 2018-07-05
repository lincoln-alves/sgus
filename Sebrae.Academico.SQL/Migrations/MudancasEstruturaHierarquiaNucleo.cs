using System.Security.Claims;
using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.MudancasEstruturaHierarquiaNucleo)]
    public class MudancasEstruturaHierarquiaNucleo : Migration
    {
        public override void Up()
        {
            Create.Column("ID_HierarquiaNucleo_New").OnTable("TB_HierarquiaNucleo").AsInt32().NotNullable().Identity();

            Create.Column("IN_Ativo").OnTable("TB_HierarquiaNucleo").AsBoolean().WithDefaultValue(true);

            // Criar relação de Hierarquia com UF.
            Create.Column("ID_UF").OnTable("TB_HierarquiaNucleo").AsCustom("TINYINT").NotNullable().WithDefaultValue(1);

            Execute.Sql(Scripts_2883.RemoverConstaintHierarquiaNucleo);

            // Remover colunas velhas de Hierarquia
            Delete.Column("ID_HierarquiaNucleo").FromTable("TB_HierarquiaNucleo");

            Rename.Column("ID_HierarquiaNucleo_New").OnTable("TB_HierarquiaNucleo").To("ID_HierarquiaNucleo");

            // Readicionar a chave primária de Hierarquia
            Execute.Sql("ALTER TABLE TB_HierarquiaNucleo ADD CONSTRAINT PK_TB_HierarquiaNucleo PRIMARY KEY(ID_HierarquiaNucleo)");

            Create.ForeignKey("FK_HierarquiaNucleo_UF")
                .FromTable("TB_HierarquiaNucleo")
                .ForeignColumn("ID_UF")
                .ToTable("TB_UF")
                .PrimaryColumn("ID_UF");
            
            Create.Column("IN_Ativo").OnTable("HT_HierarquiaNucleo").AsBoolean().WithDefaultValue(true);
            Create.Column("ID_UF").OnTable("HT_HierarquiaNucleo").AsCustom("TINYINT").NotNullable().WithDefaultValue(1);
            
            Execute.Sql(Scripts_2883.TriggersHierarquiaNucleo);

        }

        public override void Down()
        {
            Delete.ForeignKey("FK_HierarquiaNucleo_UF").OnTable("TB_HierarquiaNucleo");

            Delete.Column("IN_Ativo").FromTable("TB_HierarquiaNucleo");
            Delete.Column("ID_UF").FromTable("TB_HierarquiaNucleo");

            Delete.Column("IN_Ativo").FromTable("HT_HierarquiaNucleo");
            Delete.Column("ID_UF").FromTable("HT_HierarquiaNucleo");

            Execute.Sql(Scripts_2883.RollbackTriggersHierarquiaNucleo);
        }
    }
}