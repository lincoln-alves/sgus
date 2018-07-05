using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class SolucaoEducacionalMap : ClassMap<SolucaoEducacional>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public SolucaoEducacionalMap()
        {
            Table("TB_SOLUCAOEDUCACIONAL");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SOLUCAOEDUCACIONAL");
            References(x => x.FormaAquisicao).Column("ID_FORMAAQUISICAO");
            References(x => x.Fornecedor).Column("ID_FORNECEDOR");
            References(x => x.CategoriaConteudo).Column("ID_CATEGORIACONTEUDO");
            References(x => x.Imagem).Column("ID_FileServer").Cascade.All();
            References(x => x.TermoAceite).Column("ID_TermoAceite");
            Map(x => x.Nome).Column("NM_SOLUCAOEDUCACIONAL");
            Map(x => x.IDChaveExterna).Column("ID_CHAVEEXTERNA");
            Map(x => x.Inicio).Column("DT_Inicio");
            Map(x => x.Fim).Column("DT_Fim");
            Map(x => x.Ementa).Column("TX_EMENTA").Length(2147483647);
            Map(x => x.DataCadastro).Column("DT_CADASTRO");
            Map(x => x.TemMaterial).Column("IN_TEMMATERIAL");
            Map(x => x.Autor).Column("NM_AUTOR");
            Map(x => x.Apresentacao).Column("TX_APRESENTACAO").Length(2147483647);
            Map(x => x.Objetivo).Column("TX_OBJETIVO");
            Map(x => x.Ativo).Column("IN_ATIVO");
            Map(x => x.TeraOfertasContinuas).Column("IN_TeraOfertasContinuas");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Sequencia).Column("SQ_Sequencia");
            Map(x => x.IdNode).Column("ID_Node");
            Map(x => x.IdNodePortal).Column("ID_Node_Portal");
            Map(x => x.IntegracaoComSAS).Column("IN_IntegracaoComSAS"); 
            Map(x => x.IDEvento).Column("ID_EventoCredenciamento");
            Map(x => x.CargaHoraria).Column("QT_CargaHoraria").Nullable();

            References(x => x.UsuarioCriacao).Column("ID_UsuarioCriacao").Nullable();            
            References(x => x.UFGestor).Column("ID_UFGestor").Cascade.SaveUpdate().Nullable();

            HasMany(x => x.ListaItemTrilha).KeyColumn("ID_SOLUCAOEDUCACIONAL").AsBag().Inverse()
                .Cascade.None();
            HasMany(x => x.ListaProgramaSolucaoEducacional).KeyColumn("ID_SOLUCAOEDUCACIONAL").AsBag().Inverse()
                .Cascade.None();
            HasMany(x => x.ListaPermissao).KeyColumn("ID_SolucaoEducacional").AsBag().Inverse()
               .Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaOferta).KeyColumn("ID_SolucaoEducacional").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaTags).KeyColumn("ID_SolucaoEducacional").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaSolucaoEducacionalObrigatoria).KeyColumn("ID_SolucaoEducacional").AsBag().Inverse()
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaPreRequisito).KeyColumn("ID_SolucaoEducacional").AsBag().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaAreasTematicas).KeyColumn("ID_SolucaoEducacional").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.ListProdutosSebrae).KeyColumn("ID_SolucaoEducacional").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.ListUnidadesDemandates).KeyColumn("ID_SolucaoEducacional").NotFound.Ignore().AsBag().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
