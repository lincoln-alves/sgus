using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class TrilhaNivelPermissaoMap : ClassMap<TrilhaNivelPermissao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public TrilhaNivelPermissaoMap()
        {
            Table("TB_TrilhaNivelPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TrilhaNivelPermissao");
            References(x => x.TrilhaNivel).Column("ID_TrilhaNivel");
            References(x => x.Perfil).Column("ID_Perfil");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            References(x => x.Uf).Column("ID_UF");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
        }

    }
}
