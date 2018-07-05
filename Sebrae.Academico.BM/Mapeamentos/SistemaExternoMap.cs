using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class SistemaExternoMap : ClassMap<SistemaExterno>
    {
        public SistemaExternoMap()
        {
            Table("TB_SistemaExterno");
            LazyLoad();
            Id(x => x.ID, "ID_SistemaExterno").GeneratedBy.Identity();
            Map(x => x.Nome).Column("NM_SistemaExterno").Not.Nullable();
            Map(x => x.LinkSistemaExterno).Column("LK_SistemaExterno").Not.Nullable();
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.Descricao).Column("NM_Descricao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
            Map(x => x.Publico, "IN_Publico").Not.Nullable();
            Map(x => x.EnglishTown, "IN_EnglishTown").Nullable();
            Map(x => x.MesmaJanela, "IN_MesmaJanela").Nullable();

            HasMany(x => x.ListaPermissao).KeyColumn("ID_SistemaExterno")
              .AsBag().Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaUsuariosPermitidos).KeyColumn("ID_SistemaExterno").AsBag()
                .Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }

    }

}