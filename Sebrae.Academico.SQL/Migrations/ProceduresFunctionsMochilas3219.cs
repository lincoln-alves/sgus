using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.ProceduresFunctionsMochilas3219)]
    public class ProceduresFunctionsMochilas3219 : Migration
    {
        public override void Up()
        {

            Execute.Function("FN_POSICAO_RANKING_TRILHAS");
            Execute.Function("FN_ObterTotalMoedasTrilhaNivel");
            Execute.Function("FN_ObterStatusSolucaoSebrae");

            Execute.Procedure("SP_MOCHILA_STATUS_MISSOES");
            Execute.Procedure("SP_POSICAO_RANKING_TRILHAS");
            Execute.Procedure("SP_ATUALIZAR_TOKEN_TRILHA");
            Execute.Procedure("SP_DADOS_MOCHILA");
            Execute.Procedure("SP_OBTER_SOLUCOES");
            Execute.Procedure("SP_LIDERES_TRILHAS");
        }

        public override void Down()
        {
        }
    }
}