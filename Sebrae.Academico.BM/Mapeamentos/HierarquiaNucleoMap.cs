using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class HierarquiaNucleoMap : ClassMap<HierarquiaNucleo>
    {
        public HierarquiaNucleoMap()
        {
            Table("Tb_HierarquiaNucleo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_HierarquiaNucleo");
            Map(x => x.Nome).Column("NM_HierarquiaNucleo");
            Map(x => x.Ativo).Column("IN_Ativo");
            References(x => x.Uf).Column("ID_UF");

            HasMany(x => x.HierarquiaNucleoUsuarios).Cascade.All().KeyColumn("ID_HierarquiaNucleo").AsBag().Inverse().LazyLoad();
        }

    }
}
