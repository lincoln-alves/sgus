using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class NotificacaoEnvioPermissaoMap : ClassMap<NotificacaoEnvioPermissao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public NotificacaoEnvioPermissaoMap()
        {
            Table("TB_NotificacaoEnvioPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_NotificacaoEnvioPermissao");
            References(x => x.NotificacaoEnvio).Column("ID_NotificacaoEnvio");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            References(x => x.Uf).Column("ID_UF");
            References(x => x.Perfil).Column("ID_Perfil");
            References(x => x.Turma).Column("ID_Turma");
            References(x => x.Status).Column("ID_Status");
            References(x => x.Usuario).Column("ID_Usuario");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao").Nullable();
        }
    }
}
