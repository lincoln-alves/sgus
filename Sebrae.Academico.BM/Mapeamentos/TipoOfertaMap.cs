using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TipoOfertaMap: ClassMap<TipoOferta>
    {
        public TipoOfertaMap()
        {
            Table("TB_TipoOferta");
            LazyLoad();
            Id(x => x.ID, "ID_TipoOferta").GeneratedBy.Assigned();
            Map(x => x.Nome, "NM_TipoOferta").Nullable();
            HasMany(x => x.ListaOferta).KeyColumn("ID_TipoOferta").AsBag().Inverse().Cascade.None();
        }
    }
}
