using FluentMigrator;
using Sebrae.Academico.SQL.Enums;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AdicionarPaginaRelatorio3151)]
    public class AdicionarPaginaRelatorio3151 : Migration
    {
        public override void Up()
        {
            var scriptNovaPagina = Execute.GerarScriptInsercaoPagina("Boletim", "Boletim",
                "/Relatorios/CertificadoConhecimento/Boletim.aspx", TipoDeExpressaoAdicionarPagina.PeloCaminhoDoIrmao,
                "/Relatorios/CertificadoConhecimento/CertificacaoConhecimento.aspx");

            Execute.AdicionarPagina("/Relatorios/CertificadoConhecimento/CertificacaoConhecimento.aspx",
                TipoDeExpressaoAdicionarPagina.PeloCaminhoDoIrmao, scriptNovaPagina,
                "/Relatorios/CertificadoConhecimento/Boletim.aspx");
        }

        public override void Down()
        {
            Execute.RemoverPagina("/Relatorios/CertificadoConhecimento/Boletim.aspx" );
        }
    }
}