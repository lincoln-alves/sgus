using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class NotificacaoMap : ClassMap<Notificacao>
    {
        public NotificacaoMap()
        {
            Table("TB_Notificacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Notificacao");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.UsuarioTrilha).Column("ID_UsuarioTrilha");
            References(x => x.NotificacaoEnvio).Column("ID_NotificacaoEnvio");
            Map(x => x.Link).Column("NM_Link");
            Map(x => x.Visualizado).Column("IN_Visualizado").Not.Nullable();
            Map(x => x.DataGeracao).Column("DT_Geracao").Not.Nullable();
            Map(x => x.DataVisualizacao).Column("DT_Visualizacao");
            Map(x => x.TextoNotificacao).Column("TX_Notificacao");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.DataNotificacao).Column("DT_Notificacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
            Map(x => x.TipoNotificacao).Column("TP_Notificacao").CustomType<enumTipoNotificacao>().Not.Nullable();
        }
    }
}
