using FluentMigrator;
using Sebrae.Academico.SQL.Enums;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.EstruturaOrcamentoAnualReembolso_2)]
    public class EstruturaOrcamentoAnualReembolso : Migration
    {
        public override void Up()
        {
            Create.Table("TB_OrcamentoReembolso")
                .WithColumn("ID_OrcamentoReembolso").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("VL_Ano").AsInt32().Unique().NotNullable()
                .WithColumn("VL_Orcamento").AsDecimal(19,4).NotNullable();

            Create.Column("ID_OrcamentoReembolso").OnTable("TB_Campo").AsInt32().Nullable().ForeignKey("FK_OrcamentoReembolso_Campo", "TB_OrcamentoReembolso", "ID_OrcamentoReembolso")
                .OnDelete(System.Data.Rule.SetNull);

            var scriptLista = Execute.GerarScriptInsercaoPagina("Orçamento de Reembolso", "Orçamento de Reembolso", "/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx", TipoDeExpressaoAdicionarPagina.PeloCaminhoDoIrmao,
                "/Cadastros/Demanda/ListarDemanda.aspx");

            Execute.AdicionarPagina("/Cadastros/Demanda/ListarDemanda.aspx",
                TipoDeExpressaoAdicionarPagina.PeloCaminhoDoIrmao, scriptLista,
                "/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OrcamentoReembolso_Campo").OnTable("TB_Campo");
            Delete.Column("ID_OrcamentoReembolso").FromTable("TB_Campo");
            Delete.Table("TB_OrcamentoReembolso");
            Execute.RemoverPagina("/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx");
        }
    }
}