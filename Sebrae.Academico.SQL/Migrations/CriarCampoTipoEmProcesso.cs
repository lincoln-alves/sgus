using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CriarCampoTipoEmProcesso)]
    public class CriarCampoTipoEmProcesso : Migration
    {
        public override void Up()
        {
            Create.Column("IN_Tipo").OnTable("TB_Processo").AsInt32().NotNullable().WithDefaultValue(2);
            Create.Column("IN_Mensal").OnTable("TB_Processo").AsInt32().NotNullable().WithDefaultValue(0);
            Create.Column("DT_DiaInicio").OnTable("TB_Processo").AsInt32().Nullable();
            Create.Column("DT_DiaFim").OnTable("TB_Processo").AsInt32().Nullable();
        }
        public override void Down()
        {
            Delete.Column("IN_Tipo").FromTable("TB_Processo");
            Delete.Column("IN_Mensal").FromTable("TB_Processo");
            Delete.Column("DT_DiaInicio").FromTable("TB_Processo");
            Delete.Column("DT_DiaFim").FromTable("TB_Processo");
        }
    }
}
