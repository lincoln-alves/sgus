using FluentMigrator;
using Sebrae.Academico.SQL.Enums;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AtualizarNomePagina3151)]
    public class AtualizarNomePagina3151 : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE TB_Pagina SET TX_Nome = 'Boletim de Desempenho' WHERE TX_CaminhORelativo LIKE '/Relatorios/CertificadoConhecimento/Boletim.aspx'");
        }

        public override void Down()
        {
            Execute.RemoverPagina("/Relatorios/CertificadoConhecimento/Boletim.aspx" );
        }
    }
}