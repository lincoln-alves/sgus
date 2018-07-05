using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CampoMetaValueMap : ClassMap<CampoMetaValue>
    {
        public CampoMetaValueMap()
        {
            Table("TB_CampoMetaValue");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CampoMetaValue");
            References(d => d.Campo).Column("ID_Campo");
            References(d => d.CampoMeta).Column("ID_CampoMeta");
            Map(x => x.MetaValue).Column("NM_MetaValue");
        }
    }
}
