using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MetaIndividualMap : ClassMap<MetaIndividual>
    {

        public MetaIndividualMap()
        {
            Table("TB_MetaIndividual");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MetaIndividual");
            References(x => x.Usuario).Column("ID_Usuario");
            Map(x => x.IDChaveExterna).Column("ID_ChaveExterna");
            Map(x => x.Nome).Column("NM_MetaIndividual").Not.Nullable();
            Map(x => x.DataValidade).Column("DT_Validade").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            HasMany(x => x.ListaItensMetaIndividual).KeyColumn("ID_MetaIndividual").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
