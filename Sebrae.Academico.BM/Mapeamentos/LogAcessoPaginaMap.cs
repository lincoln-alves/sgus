using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    /// <summary>
    /// Mapeamento da classe referente a Log de Acessos a uma página.
    /// <obs>Esta tabela não possui integridade referencial.</obs>
    /// </summary>
    public sealed class LogAcessoPaginaMap : ClassMap<LogAcessoPagina>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogAcessoPaginaMap()
        {
            Table("LG_AcessoPagina");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("ID_AcessoPagina");

            Map(x => x.IP).Column("TX_IP").Length(20);
            Map(x => x.QueryString).Column("TX_QueryString").Length(30);
            Map(x => x.Acao).Column("VL_Acao").CustomType<enumAcaoNaPagina>();
            Map(x => x.DTSolicitacao).Column("DT_Solicitacao").Not.Nullable();

            References(x => x.IDUsuario).Column("ID_Usuario");
            References(x => x.Pagina).Column("ID_Pagina");

    }

    }
}