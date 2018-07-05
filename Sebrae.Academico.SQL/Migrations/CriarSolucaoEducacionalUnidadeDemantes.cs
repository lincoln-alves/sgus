using FluentMigrator;
using System;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CriarSolucaoEducacionalUnidadeDemantes)]
    public class CriarSolucaoEducacionalUnidadeDemantes : Migration
    {
        public override void Down()
        {
            Delete.Table("TB_SolucaoEducacionalUnidadeDemantes");
        }

        public override void Up()
        {
            Create.Table("TB_SolucaoEducacionalUnidadeDemantes").WithColumn("ID_SolucaoEducacionalUnidadeDemantes").AsInt32().Identity().PrimaryKey().NotNullable();
            Create.Column("DT_Alteracao").OnTable("TB_SolucaoEducacionalUnidadeDemantes").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);
            Create.Column("ID_Cargo").OnTable("TB_SolucaoEducacionalUnidadeDemantes").AsInt32().NotNullable();
            Create.Column("ID_SolucaoEducacional").OnTable("TB_SolucaoEducacionalUnidadeDemantes").AsInt32().NotNullable();
        }
    }
}
