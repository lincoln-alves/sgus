using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    /// <summary>
    /// Mapeamento da classe referente a Log do Fale Conosco.
    /// <obs>Esta tabela não possui integridade referencial.</obs>
    /// </summary>
    public sealed class LogFaleConoscoMap : ClassMap<LogFaleConosco>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public LogFaleConoscoMap()
        {
            Table("LG_FaleConosco");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_FaleConosco");
            Map(x => x.CPF).Column("TX_CPF").Not.Nullable().Length(11);
            Map(x => x.Nome).Column("TX_Nome").Length(255);
            Map(x => x.Email).Column("TX_Email").Length(255);
            Map(x => x.Assunto).Column("TX_Assunto").Length(255);
            Map(x => x.Mensagem).Column("TX_Mensagem").Length(2000);
            Map(x => x.IP).Column("TX_IP").Length(255);
            Map(x => x.DataSolicitacao).Column("DT_Solicitacao").Not.Nullable();
        }

    }

}