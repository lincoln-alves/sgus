using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class MensageriaParametroMap: ClassMap<MensageriaParametros>
    {
        public MensageriaParametroMap()
        {
            Table("TB_MensageriaParametro");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MensageriaParametro");
            Map(x => x.DiaAviso).Column("QT_DiaAviso").Not.Nullable();
            Map(x => x.NomeArquivoTemplate).Column("NM_ArquivoTemplate");
            Map(x => x.NotificaMatriculaTurma).Column("IN_NotificaMatriculaTurma").Not.Nullable();
            Map(x => x.NotificaUsuarioTrilha).Column("IN_NotifcaUsuarioTrilha").Not.Nullable();
            Map(x => x.Repetir).Column("IN_Repetir").Not.Nullable();
            Map(x => x.EnviarEmail).Column("IN_EnviaEmail").Not.Nullable();
            Map(x => x.EnviarNotificacao).Column("IN_EnviaNotificacao").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
