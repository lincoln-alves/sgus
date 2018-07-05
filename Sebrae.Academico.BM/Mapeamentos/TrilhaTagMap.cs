using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TrilhaTagMap : ClassMap<TrilhaTag>
    {
        public TrilhaTagMap()
        {
            Table("TB_TRILHATAG");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaTag");
            References(x => x.Tag, "ID_TAG");
            References(x => x.Trilha, "ID_TRILHA");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }
}
