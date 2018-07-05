using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class ConfiguracaoPagamentoMap: ClassMap<ConfiguracaoPagamento>
    {
        public ConfiguracaoPagamentoMap()
        {
            Table("TB_ConfiguracaoPagamento");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ConfiguracaoPagamento");
            References(x => x.TipoPagamento).Column("ID_TipoPagamento").Cascade.None().Not.Nullable();
            Map(x => x.BloqueiaAcesso).Column("IN_BloqueiaAcesso");
            Map(x => x.DataInicioCompetencia).Column("DT_InicioCompetencia");
            Map(x => x.DataFimCompetencia).Column("DT_FimCompetencia");
            Map(x => x.QuantidadeDiasInadimplencia).Column("QT_DiasInadimplencia");
            Map(x => x.QuantidadeDiasRenovacao).Column("QT_DiasRenovacao");
            Map(x => x.QuantidadeDiasValidade).Column("QT_DiasValidade");
            Map(x => x.QuantidadeDiasPagamento).Column("QT_DiasPagamento");
            Map(x => x.Recursiva).Column("IN_Recursiva");
            Map(x => x.ValorAPagar).Column("VL_ValorAPagar");
            Map(x => x.Ativo).Column("IN_Ativo");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.Nome).Column("NM_ConfiguracaoPagamento").Not.Nullable();
            Map(x => x.TextoTermoAdesao).Column("TX_TERMOADESAO").Length(2147483647);


            HasMany(x => x.ListaConfiguracaoPagamentoPublicoAlvo).KeyColumn("ID_ConfiguracaoPagamento").AsBag()
                   .Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaPagamento).KeyColumn("ID_ConfiguracaoPagamento");

            
        }
    }
}


