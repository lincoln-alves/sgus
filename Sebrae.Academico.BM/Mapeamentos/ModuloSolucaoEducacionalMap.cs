using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ModuloSolucaoEducacionalMap : ClassMap<ModuloSolucaoEducacional>
    {
        public ModuloSolucaoEducacionalMap()
        {
            Table("TB_ModuloSolucaoEducacional");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ModuloSolucaoEducacional");
            References(x => x.Modulo).Column("ID_Modulo");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional");
            Map(x => x.Ordem).Column("IN_Ordem");
        }
    }
}
