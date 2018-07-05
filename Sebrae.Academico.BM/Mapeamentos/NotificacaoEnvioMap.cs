using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class NotificacaoEnvioMap : ClassMap<NotificacaoEnvio>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public NotificacaoEnvioMap()
        {
            Table("TB_NOTIFICACAOENVIO");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_NOTIFICACAOENVIO");
            Map(x => x.Texto).Column("TX_Notificacao");
            Map(x => x.Link).Column("NM_Link");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");

            References(x => x.Uf).Column("ID_UF");

            HasMany(x => x.ListaPermissao).KeyColumn("ID_NotificacaoEnvio").AsBag().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Notificacoes).KeyColumn("ID_NotificacaoEnvio").AsBag();
        }
    }
}
