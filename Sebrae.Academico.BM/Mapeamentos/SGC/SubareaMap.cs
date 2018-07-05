using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.SGC;

namespace Sebrae.Academico.BM.Mapeamentos.SGC
{
    public sealed class SubareaMap : ClassMap<Subarea>
    {
        public SubareaMap()
        {
            Table("TB_SGC_Subarea");
            LazyLoad();
            Id(x => x.ID).Column("IDSUBAREA");
            Map(x => x.Nome).Column("NOMESUBAREA");
            Map(x => x.CodigoSituacao).Column("CODSITUACAOSUBAREA");
            Map(x => x.DescricaoSituacao).Column("DESCSITUACAOSUBAREA");
            Map(x => x.Alteracao).Column("DATAALTERACAO");

            References(x => x.Area).Column("IDAREA").Cascade.None().Not.Nullable().Not.LazyLoad();

            HasMany(x => x.CredenciadoArea).KeyColumn("IDSUBAREA");

            HasManyToMany(a => a.OfertaPermissoes)
                .Table("TB_OfertaPermissaoSubarea")
                .ParentKeyColumn("IDSUBAREA")
                .ChildKeyColumn("ID_OfertaPermissao")
                .Cascade.None();
        }
    }
}
