using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class MetaFmMap : ClassMap<MetaFm>
    {
        public MetaFmMap()
        {
            Table("TB_MetaFm");
            LazyLoad();
            Id(x => x.ID).Column("ID_MetaFm");
            Map(x => x.Nome).Column("NM_Nome");
            Map(x => x.Numero).Column("VL_Numero");
            HasMany(x => x.Categorias).KeyColumn("ID_MetaFm");
        }
    }
}