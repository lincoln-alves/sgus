using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SolucaoEducacionalTagsMap: ClassMap<SolucaoEducacionalTags>
    {
        public SolucaoEducacionalTagsMap()
        {
            Table("TB_SolucaoEducacionalTag");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalTag");
            References(x => x.Tag, "ID_Tag");
            References(x => x.SolucaoEducacional, "ID_SolucaoEducacional");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
