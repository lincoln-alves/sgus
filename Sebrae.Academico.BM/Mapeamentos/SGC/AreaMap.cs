using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.BM.Mapeamentos.SGC
{
    public sealed class AreaMap : ClassMap<Area>
    {
        public AreaMap()
        {
            Table("TB_SGC_Area");
            LazyLoad();
            Id(x => x.ID).Column("IDAREA");
            Map(x => x.Nome).Column("NOMEAREA");
            Map(x => x.CodigoSituacao).Column("CODSITUACAOAREA");
            Map(x => x.DescricaoSituacao).Column("DESCSITUACAOAREA");
            Map(x => x.Alteracao).Column("DATAALTERACAO");

            HasMany(x => x.Subareas).KeyColumn("IDAREA").Cascade.None().Not.KeyNullable();
            HasMany(x => x.CredenciadoArea).KeyColumn("IDAREA");
        }
    }
}
