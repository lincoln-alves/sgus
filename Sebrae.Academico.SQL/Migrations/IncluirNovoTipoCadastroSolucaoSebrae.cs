using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.IncluirNovoTipoCadastroSolucaoSebrae)]
    public class IncluirNovoTipoCadastroSolucaoSebrae : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO TB_TipoItemTrilha (ID_TipoItemTrilha, NM_TipoItemTrilha)  VALUES (5, 'Conheci Game')");
        }

        public override void Down()
        {
            Execute.Sql("DELETE t FROM TB_TipoItemTrilha t WHERE t.ID_TipoItemTrilha = 5");
        }
    }
}
