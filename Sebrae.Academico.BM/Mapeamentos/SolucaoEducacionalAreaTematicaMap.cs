using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SolucaoEducacionalAreaTematicaMap: ClassMap<SolucaoEducacionalAreaTematica>
    {
        public SolucaoEducacionalAreaTematicaMap()
        {
            Table("TB_SolucaoEducacionalAreaTematica");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalAreaTematica");
            References(x => x.AreaTematica, "ID_AreaTematica");
            References(x => x.SolucaoEducacional, "ID_SolucaoEducacional");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
