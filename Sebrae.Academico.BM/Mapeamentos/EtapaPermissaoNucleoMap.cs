using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class EtapaPermissaoNucleoMap : ClassMap<EtapaPermissaoNucleo>
    {
        public EtapaPermissaoNucleoMap()
        {
            Table("TB_EtapaPermissaoNucleo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EtapaPermissaoNucleo");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");

            References(x => x.Etapa).Column("ID_Etapa");
            References(x => x.HierarquiaNucleoUsuario).Column("ID_HierarquiaNucleoUsuario");

            HasMany(x => x.PermissoesNucleoEtapaResposta).KeyColumn("ID_EtapaPermissaoNucleo").AsBag().Cascade.All();
        }
    }
}
