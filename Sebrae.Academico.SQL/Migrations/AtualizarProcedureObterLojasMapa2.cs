using FluentMigrator;
using Sebrae.Academico.SQL.Extensions;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.AtualizarProcedureObterLojasMapa2)]
    public class AtualizarProcedureObterLojasMapa2 : Migration
    {
        public override void Up()
        {
            // Drop if exists
            Execute.Sql("IF OBJECT_ID('dbo.FN_ObterMoedasConquistadasPontoSebrae', 'FN') IS NOT NULL DROP FUNCTION [dbo].[FN_ObterMoedasConquistadasPontoSebrae]");
            Execute.Function("FN_ObterMoedasConquistadasPontoSebrae");
            // Drop if exists
            Execute.Sql("IF OBJECT_ID('dbo.SP_OBTER_LOJAS_MAPA', 'P') IS NOT NULL DROP PROCEDURE [dbo].[SP_OBTER_LOJAS_MAPA]");
            Execute.Procedure("SP_OBTER_LOJAS_MAPA");
        }

        public override void Down()
        {
        }
    }
}
