using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.LogAcessoPagina)]
    public class LogAcessoPagina : Migration
    {
        public override void Up()
        {
            Delete.Column("TX_Link").FromTable("LG_AcessoPagina");
            Delete.Column("TX_Nome").FromTable("LG_AcessoPagina");

            Create.Column("ID_AcessoPagina").OnTable("LG_AcessoPagina").AsInt32().Identity().PrimaryKey().NotNullable();

            Create.Column("TX_QueryString").OnTable("LG_AcessoPagina").AsString(40).Nullable();
            Create.Column("VL_Acao").OnTable("LG_AcessoPagina").AsInt32().Nullable();
            Create.Column("ID_Pagina").OnTable("LG_AcessoPagina").AsInt32().Nullable();
        }

        public override void Down()
        {
            Create.Column("TX_Link").OnTable("LG_AcessoPagina").AsString(500).NotNullable();
            Create.Column("TX_Nome").OnTable("LG_AcessoPagina").AsString(500).Nullable();

            Delete.Column("ID_AcessoPagina").FromTable("LG_AcessoPagina");
            Delete.Column("TX_QueryString").FromTable("LG_AcessoPagina");
            Delete.Column("VL_Acao").FromTable("LG_AcessoPagina");
            Delete.Column("ID_Pagina").FromTable("LG_AcessoPagina");
        }
    }
}
