using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ProgramaTagMap : ClassMap<ProgramaTag>
    {
        public ProgramaTagMap()
        {
            Table("TB_PROGRAMATAG");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PROGRAMATAG");
            References(x => x.Programa).Column("ID_PROGRAMA");
            References(x => x.Tag).Column("ID_TAG");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }

    }
}
