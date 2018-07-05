using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TrilhaCategoriaConteudoMap : ClassMap<TrilhaCategoriaConteudo>
    {
        public TrilhaCategoriaConteudoMap()
        {
            Table("TB_TrilhaCategoriaConteudo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaCategoriaConteudo");
            References(x => x.CategoriaConteudo, "ID_CategoriaConteudo");
            References(x => x.Trilha, "ID_Trilha");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
