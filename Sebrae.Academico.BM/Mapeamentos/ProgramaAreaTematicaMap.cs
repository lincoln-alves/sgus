using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ProgramaAreaTematicaMap: ClassMap<ProgramaAreaTematica>
    {
        public ProgramaAreaTematicaMap()
        {
            Table("TB_ProgramaAreaTematica");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ProgramaAreaTematica");
            References(x => x.AreaTematica, "ID_AreaTematica");
            References(x => x.Programa, "ID_Programa");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
