using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    
    public class MetaInstitucionalMap : ClassMap<MetaInstitucional>
    {

        public MetaInstitucionalMap()
        {
            Table("TB_MetaInstitucional");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MetaInstitucional");
            Map(x => x.Nome).Column("NM_MetaInstitucional").Not.Nullable();
            Map(x => x.DataInicioCiclo).Column("DT_InicioCiclo").Not.Nullable();
            Map(x => x.DataFimCiclo).Column("DT_FimCiclo").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            HasMany(x => x.ListaItensMetaInstitucional)
                .KeyColumn("ID_MetaInstitucional").AsBag()
                   .Inverse().LazyLoad().Cascade.AllDeleteOrphan();  
        }
    }
}
