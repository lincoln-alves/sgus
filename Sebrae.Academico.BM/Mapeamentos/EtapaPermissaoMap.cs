using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class EtapaPermissaoMap : ClassMap<EtapaPermissao>
    {
        public EtapaPermissaoMap()
        {
            Table("TB_EtapaPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Permissao");
            References(x => x.Etapa).Column("ID_Etapa").Not.Nullable().ReadOnly();
            References(x => x.Usuario).Column("ID_PermissaoUsuario");
            References(x => x.Perfil).Column("ID_PermissaoPerfil");
            References(x => x.NivelOcupacional).Column("ID_PermissaoOcupacao");
            References(x => x.Uf).Column("ID_PermissaoUF");
            Map(x => x.Notificar).Column("IN_Notificar");
            Map(x => x.Analisar).Column("IN_Analisar");

            Map(x => x.ChefeImediato).Column("IN_ChefeImediato");
            Map(x => x.DiretorCorrespondente).Column("IN_DiretorCorrespondente");
            Map(x => x.GerenteAdjunto).Column("IN_GerenteAdjunto");
            Map(x => x.Solicitante).Column("IN_Solicitante");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
