using FluentMigrator;
using Sebrae.Academico.SQL.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CriarEstruturaPercentagemDemandas3705)]
    public class CriarEstruturaPercentagemDemandas3705 : Migration
    {
        public override void Up()
        {
            Create.Table("TB_CampoPorcentagem").WithColumn("ID_CampoPorcentagem").AsInt32().Identity().PrimaryKey().NotNullable();
            Create.Column("ID_Campo").OnTable("TB_CampoPorcentagem").AsInt32().NotNullable();
            Create.Column("ID_CampoRelacionado").OnTable("TB_CampoPorcentagem").AsInt32().NotNullable();
            Create.Column("DT_UltimaAtualizacao").OnTable("TB_CampoPorcentagem").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);
        }

        public override void Down()
        {
            Delete.Table("TB_CampoPorcentagem");
        }
    }
}
