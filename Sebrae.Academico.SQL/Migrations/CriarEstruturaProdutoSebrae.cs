using FluentMigrator;
using Sebrae.Academico.SQL.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CriarEstruturaProdutoSebrae)]
    public class CriarEstruturaProdutoSebrae : Migration
    {
        public override void Down()
        {
            Delete.Table("TB_ProdutoSebrae");
            Delete.Table("TB_SolucaoEducacionalProdutoSebrae");
        }

        public override void Up()
        {
            Create.Table("TB_ProdutoSebrae").WithColumn("ID_ProdutoSebrae").AsInt32().Identity().PrimaryKey().NotNullable();
            Create.Column("NM_Nome").OnTable("TB_ProdutoSebrae").AsString(100).NotNullable();
            Create.Column("DT_Alteracao").OnTable("TB_ProdutoSebrae").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);
            Execute.Sql(Scripts_3400.CommitCriarEstruturaProdutoSebrae);

            Create.Table("TB_SolucaoEducacionalProdutoSebrae").WithColumn("ID_SolucaoEducacionalProdutoSebrae").AsInt32().Identity().PrimaryKey().NotNullable();
            Create.Column("ID_ProdutoSebrae").OnTable("TB_SolucaoEducacionalProdutoSebrae").AsInt32().NotNullable();
            Create.Column("ID_SolucaoEducacional").OnTable("TB_SolucaoEducacionalProdutoSebrae").AsInt32().NotNullable();
            Create.Column("DT_UltimaAtualizacao").OnTable("TB_SolucaoEducacionalProdutoSebrae").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);
        }
    }
}
