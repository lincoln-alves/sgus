using FluentMigrator;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AlteracoesDemandas)]
    public class AlteracoesEmDemandas : Migration
    {
        public override void Up()
        {
            Execute.Sql(Scripts_3696.AdicionandoColunasEmDemandas);
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE TB_Etapa DROP COLUMN TX_NomeBotaoAjuste;");
            Execute.Sql("ALTER TABLE TB_Etapa DROP COLUMN IN_PodeSerReprovada;");
        }
    }
}
