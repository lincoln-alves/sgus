using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class PublicacaoSaberUsuarioMap : ClassMap<PublicacaoSaberUsuario>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public PublicacaoSaberUsuarioMap()
        {
            Table("TB_PublicacaoSaberUsuario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PublicacaoSaberUsuario");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.PublicacaoSaber).Column("ID_PublicacaoSaber");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }

    }
}