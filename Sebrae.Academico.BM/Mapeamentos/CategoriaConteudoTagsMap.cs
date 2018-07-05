using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class CategoriaConteudoTagsMap : ClassMap<CategoriaConteudoTags>
    {
        public CategoriaConteudoTagsMap()
        {
            Table("TB_CATEGORIACONTEUDOTag");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CategoriaConteudoTag");
            References(x => x.Tag, "ID_TAG");
            References(x => x.CategoriaConteudo, "ID_CategoriaConteudo");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
