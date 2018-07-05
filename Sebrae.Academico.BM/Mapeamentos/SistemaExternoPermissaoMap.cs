using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{

    public sealed class SistemaExternoPermissaoMap : ClassMap<SistemaExternoPermissao>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public SistemaExternoPermissaoMap()
        {
            Table("TB_SistemaExternoPermissao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SistemaExternoPermissao");
            References(x => x.SistemaExterno).Column("ID_SistemaExterno");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            References(x => x.Uf).Column("ID_UF");
            References(x => x.Perfil).Column("ID_Perfil");
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
        }

    }

}