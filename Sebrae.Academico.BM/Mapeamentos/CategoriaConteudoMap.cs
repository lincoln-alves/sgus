using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class CategoriaConteudoMap : ClassMap<CategoriaConteudo>
    {
        public CategoriaConteudoMap()
        {
            Table("TB_CATEGORIACONTEUDO");
            //LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_CATEGORIACONTEUDO");
            Map(x => x.Nome).Column("NM_CATEGORIACONTEUDO");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_USUARIOATUALIZACAO");
            Map(x => x.IdNode).Column("ID_Node");
            Map(x => x.PossuiFiltroCategorias).Column("IN_FiltroStatus");
            Map(x => x.Apresentacao).Column("TX_APRESENTACAO").Length(2147483647);
            Map(x => x.Descricao).Column("TX_Descricao");
            Map(x => x.Sigla).Column("NM_Sigla");
            Map(x => x.LiberarInscricao).Column("IN_LiberarInscricao");
            Map(x => x.PossuiStatus).Column("IN_PossuiStatus");
            Map(x => x.PossuiAreas).Column("IN_PossuiAreas");
            HasMany(x => x.ListaSolucaoEducacional).KeyColumn("ID_CATEGORIACONTEUDO").AsBag().Inverse().Cascade.None().Fetch.Select();
            HasMany(x => x.ListaTrilha).KeyColumn("ID_CATEGORIACONTEUDO").AsBag().Inverse().Cascade.None().Fetch.Select();
            HasMany(x => x.ListaCategoriaConteudoUF).KeyColumn("ID_CategoriaConteudo").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();
            References(x => x.MetaFm, "ID_MetaFm").Nullable();

            //HasMany(x => x.ListaUsuarioCategoriaConteudo).KeyColumn("ID_CategoriaConteudo").Cascade.None().Inverse();

            HasManyToMany(a => a.ListaUsuario)
                .Table("TB_UsuarioCategoriaConteudo")
                .ParentKeyColumn("ID_CategoriaConteudo")
                .ChildKeyColumn("ID_Usuario");

            HasOne(x => x.TermoAceiteCategoriaCounteudo).ForeignKey("ID_CategoriaConteudo").Cascade.None();

            References(x => x.CategoriaConteudoPai).Column("ID_CATEGORIACONTEUDOPAI");
            References(x => x.UF).Column("ID_UF");

            HasMany(x => x.ListaCategoriaConteudoFilhos).Cascade.All().KeyColumn("ID_CATEGORIACONTEUDOPAI");

            HasMany(x => x.ListaPermissao).KeyColumn("ID_CATEGORIACONTEUDO").AsBag().Inverse()
               .LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaTags).KeyColumn("ID_CATEGORIACONTEUDO").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasManyToMany(a => a.ListaStatusMatricula)
                .Table("TB_CategoriaConteudoStatusMatricula")
                .ParentKeyColumn("ID_CategoriaConteudo")
                .ChildKeyColumn("ID_StatusMatricula");
        }
    }
}
