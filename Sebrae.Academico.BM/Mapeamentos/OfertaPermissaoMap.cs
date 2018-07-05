using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class OfertaPermissaoMap : ClassMap<OfertaPermissao>
    {
        public OfertaPermissaoMap()
        {
            Table("TB_OfertaPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_OfertaPermissao");
            References(x => x.Oferta).Column("ID_Oferta").Not.Nullable();
            References(x => x.Uf).Column("ID_UF");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            References(x => x.Perfil).Column("ID_Perfil");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.QuantidadeVagasPorEstado).Column("QT_VagasPorUF");

            HasManyToMany(a => a.Subareas)
                .Table("TB_OfertaPermissaoSubarea")
                .ParentKeyColumn("ID_OfertaPermissao")
                .ChildKeyColumn("IDSUBAREA");
        }
    }
}
