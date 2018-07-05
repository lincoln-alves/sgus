using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TipoItemTrilhaMap : ClassMap<TipoItemTrilha>
    {
        public TipoItemTrilhaMap()
        {
            Table("TB_TipoItemTrilha");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TipoItemTrilha");
            Map(x => x.Nome).Column("NM_TipoItemTrilha");

            HasMany(x => x.ItensTrilha).KeyColumn("ID_ItemTrilha").AsBag()
                   .Inverse().Cascade.AllDeleteOrphan();
        }
    }
}