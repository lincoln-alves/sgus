using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CampoMetaMap : ClassMap<CampoMeta>
    {
        public CampoMetaMap()
        {
            Table("TB_CampoMeta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CampoMeta");
            //References(d => d.Etapa).Column("ID_Etapa");            
            Map(x => x.CampoTipoDado).Column("VL_CampoTipoDado");
            Map(x => x.CampoTipo).Column("VL_CampoTipo");
            Map(x => x.MetaNome).Column("NM_MetaNome");
            Map(x => x.MetaKey).Column("NM_MetaKey");
            Map(x => x.MetaDescription).Column("NM_MetaDescription");

            HasMany(x => x.ListaMetaValues).KeyColumn("ID_CampoMeta").AsBag().Inverse().Cascade.All();
        }
    }
}
