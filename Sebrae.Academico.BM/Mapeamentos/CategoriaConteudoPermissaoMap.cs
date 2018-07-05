using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class CategoriaConteudoPermissaoMap : ClassMap<CategoriaConteudoPermissao>
    {
        public CategoriaConteudoPermissaoMap()
        {
            Table("TB_CATEGORIACONTEUDOPERMISSAO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CATEGORIACONTEUDOPermissao");
            References(x => x.CategoriaConteudo).Column("ID_CATEGORIACONTEUDO").LazyLoad();
            References(x => x.Uf).Column("ID_UF").LazyLoad();
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional").LazyLoad();
            References(x => x.Perfil).Column("ID_Perfil").LazyLoad();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
