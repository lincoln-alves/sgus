using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class LogAcessoSolucaoEducacionalMap : ClassMap<LogAcessoSolucaoEducacional>
    {
        public LogAcessoSolucaoEducacionalMap()
        {
            Table("LG_AcessoSolucaoEducacional");
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_AcessoSolucaoEducacional");
            Map(x => x.ID_SolucaoEducacional).Column("ID_SolucaoEducacional");
            Map(x => x.ID_Oferta).Column("ID_Oferta");
            Map(x => x.ID_Turma).Column("ID_Turma");
            Map(x => x.QuantidadeDeAcessos).Column("QuantidadeDeAcessos");
            Map(x => x.DataAcesso).Column("DT_Acesso");
        }
    }
}
