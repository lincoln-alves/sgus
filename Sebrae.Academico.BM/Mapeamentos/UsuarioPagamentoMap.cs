using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class UsuarioPagamentoMap : ClassMap<UsuarioPagamento>
    {
        public UsuarioPagamentoMap()
        {
            Table("TB_UsuarioPagamento");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioPagamento");
            References(x => x.Usuario).Column("ID_Usuario").Cascade.None().Not.Nullable();
            References(x => x.ConfiguracaoPagamento).Column("ID_ConfiguracaoPagamento");
            Map(x => x.DataInicioVigencia).Column("DT_InicioVigencia").Not.Nullable();
            Map(x => x.DataFimVigencia).Column("DT_FimVigencia").Not.Nullable();
            Map(x => x.DataPagamento).Column("DT_Pagamento");
            Map(x => x.ValorPagamento).Column("VL_Pago").Precision(8).Scale(4);
            Map(x => x.DataInicioRenovacao).Column("DT_InicioRenovacao").Not.Nullable();
            Map(x => x.DataMaxInadimplencia).Column("DT_MaxInadimplencia").Not.Nullable();
            Map(x => x.NossoNumero).Column("CD_Informacao").Length(255);
            Map(x => x.PagamentoEfetuado).Column("IN_Pago").Not.Nullable();
            Map(x => x.FormaPagamento, "TP_FormaPagamento").CustomType<enumFormaPagamento>();
            Map(x => x.DataAceiteTermoAdesao).Column("DT_AceiteTermoAdesao");
            Map(x => x.DataVencimento).Column("DT_Vencimento");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            Map(x => x.TextoDaAutenticacaoBancaria).Column("TX_AutenticacaoBancaria");
            Map(x => x.PagamentoInformado).Column("IN_PagamentoInformado");
            Map(x => x.DataPagamentoInformado).Column("DT_PagamentoInformado");
            Map(x => x.PagamentoConfirmado).Column("IN_PagamentoConfirmado");
            Map(x => x.PagamentoEnviadoBanco).Column("IN_PagamentoEnviadoBanco");

        }
    }
}
