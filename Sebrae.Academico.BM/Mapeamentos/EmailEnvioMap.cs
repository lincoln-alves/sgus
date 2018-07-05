using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class EmailEnvioMap : ClassMap<EmailEnvio>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public EmailEnvioMap()
        {
            Table("TB_EmailEnvio");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EmailEnvio");
            Map(x => x.Texto).Column("TX_Email");
            Map(x => x.Assunto).Column("TX_Assunto");
            Map(x => x.DataGeracao).Column("DT_Geracao");
            Map(x => x.Processado).Column("IN_Processado");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            References(x => x.Uf).Column("ID_UF");

            HasMany(x => x.ListaPermissao).KeyColumn("ID_EmailEnvio").AsBag().LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaEmailsGerados).KeyColumn("ID_EmailEnvio").AsBag().LazyLoad().Inverse().Not.KeyNullable().Cascade.All();
        }
    }
}
