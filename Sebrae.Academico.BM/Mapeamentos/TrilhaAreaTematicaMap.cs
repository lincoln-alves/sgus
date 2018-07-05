using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class TrilhaAreaTematicaMap : ClassMap<TrilhaAreaTematica>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaAreaTematicaMap()
        {
            Table("TB_TrilhaAreaTematica");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaAreaTematica");
            References(x => x.AreaTematica, "ID_AreaTematica");
            References(x => x.Trilha, "ID_Trilha");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }

    }
}
