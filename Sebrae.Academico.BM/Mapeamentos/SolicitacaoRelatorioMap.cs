using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class SolicitacaoRelatorioMap : ClassMap<SolicitacaoRelatorio>
    {
        public SolicitacaoRelatorioMap()
        {
            Table("TB_SolicitacaoRelatorio");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolicitacaoRelatorio");
            References(d => d.Usuario).Column("ID_Usuario");
            References(x => x.Arquivo).Column("ID_FileServer");
            Map(x => x.DataSolicitacao).Column("DT_Solicitacao");
            Map(x => x.DataGeracao).Column("DT_Geracao");
            Map(x => x.Nome).Column("NM_Solicitacao");
            Map(x => x.NomeAmigavel).Column("NM_SolicitacaoAmigavel");
            Map(x => x.Saida).Column("VL_Saida");
            Map(x => x.Baixado).Column("IN_Baixado");
            Map(x => x.Falha).Column("IN_Falha");
            Map(x => x.Descricao).Column("TX_Descricao");
            Map(x => x.QuantidadeRegistros).Column("QT_Registros").CustomSqlType("BIGINT");
        }
    }
}
