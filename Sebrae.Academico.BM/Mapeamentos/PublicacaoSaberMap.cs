using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class PublicacaoSaberMap : ClassMap<PublicacaoSaber>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public PublicacaoSaberMap()
        {

            Table("TB_PublicacaoSaber");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_PublicacaoSaber");
            References(x => x.UF).Column("ID_UF");
            Map(x => x.IDChaveExterna).Column("ID_ChaveExterna").Not.Nullable().Precision(10);
            Map(x => x.Publicado).Column("IN_Publicado").Not.Nullable().Precision(10);
            Map(x => x.TextoResenha).Column("TX_Resenha").Not.Nullable().Length(2000);
            Map(x => x.TextoAssunto).Column("TX_Assunto").Length(2000);
            Map(x => x.DataPublicacao).Column("DT_Publicacao");
            Map(x => x.TextoLinkCapa).Column("TX_LinkCapa").Length(500);
            Map(x => x.TextoTitulo).Column("TX_Titulo").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            HasMany(x => x.ListaPublicacaoSaberUsuario).KeyColumn("ID_PublicacaoSaber").AsBag().Inverse()
              .LazyLoad().Cascade.AllDeleteOrphan();

            
        }

    }
}