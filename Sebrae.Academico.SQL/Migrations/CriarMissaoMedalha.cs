using FluentMigrator;
using System;
using Sebrae.Academico.SQL.Properties;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.CriarMissaoMedalha)]
    public class CriarMissaoMedalha : Migration
    {
        public override void Down()
        {
            Execute.Sql(Scripts_4189.RemoverTabelaMissaoMedalha);
        }

        public override void Up()
        {
            Execute.Sql(Scripts_4189.CriarTabelaMissaoMedalha);
        }
    }
}
