using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class AreaTematicaMap : ClassMap<AreaTematica>
    {
        public AreaTematicaMap()
        {
            Table("TB_AreaTematica");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_AreaTematica");
            Map(x => x.Nome).Column("NM_AreaTematica");
            Map(x => x.Icone).Column("NM_Icone");
            Map(x => x.Apresentacao).Column("TX_Apresentacao");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            HasMany(x => x.ListaPermissao).KeyColumn("ID_AreaTematica").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            Map(x => x.IdNodePortal).Column("ID_Node_Portal");
        }
    }
}
