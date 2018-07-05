using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class ItemTrilhaMap : ClassMap<ItemTrilha>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public ItemTrilhaMap()
        {
            Table("TB_ITEMTRILHA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ITEMTRILHA");
            References(x => x.SolucaoEducacional).Column("ID_SOLUCAOEDUCACIONAL");
            References(x => x.TrilhaTopicoTematico).Column("ID_TRILHATOPICOTEMATICO").Cascade.None().LazyLoad();
            References(x => x.FormaAquisicao).Column("ID_FORMAAQUISICAO").Cascade.None().LazyLoad();
            References(x => x.Usuario).Column("ID_USUARIO").Cascade.None().LazyLoad();
            Map(x => x.QuantidadePontosParticipacao).Column("QT_PONTOS");
            Map(x => x.Nome).Column("NM_ITEMTRILHA").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.DataCriacao).Column("DT_CRIACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Ativo).Column("IN_ATIVO");
            Map(x => x.PermiteReenvioArquivo).Column("IN_PermiteEnvioArquivo");            
            Map(x => x.Aprovado).Column("IN_Aprovado").CustomType<enumStatusSolucaoEducacionalSugerida>();
            Map(x => x.Local).Column("TX_Local");
            Map(x => x.LinkConteudo).Column("LK_Conteudo");
            Map(x => x.ReferenciaBibliografica).Column("TX_ReferenciaBibliografica");
            Map(x => x.SolucaoObrigatoria).Column("IN_SolucaoObrigatoria");
            Map(x => x.Observacao).Column("TX_Observacao");
            Map(x => x.CargaHoraria).Column("QT_CargaHoraria");
            Map(x => x.Moedas).Column("VL_Moedas");
            Map(x => x.FaseJogo).Column("VL_FaseJogo").CustomType<enumFaseJogo>();
            Map(x => x.QuantidadeAcertosTema).Column("QT_AcertosTema");
            Map(x => x.ID_TemaConheciGame).Column("ID_TemaConheciGame");

            HasMany(x => x.ListaItemTrilhaParticipacao).KeyColumn("ID_ItemTrilha").AsBag()
                   .Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Avaliacoes).KeyColumn("ID_ItemTrilha").AsBag()
                 .Inverse().Cascade.AllDeleteOrphan().LazyLoad();

            References(x => x.FileServer).Column("ID_FileServer").Cascade.All();

            References(x => x.Objetivo).Column("ID_Objetivo");

            References(x => x.Tipo).Column("ID_TipoItemTrilha");
            References(x => x.Questionario).Column("ID_Questionario").Fetch.Join();
            References(x => x.SolucaoEducacionalAtividade).Column("ID_SolucaoEducacionalAtividade");
            References(x => x.Missao).Column("ID_Missao");
        }
    }
}
