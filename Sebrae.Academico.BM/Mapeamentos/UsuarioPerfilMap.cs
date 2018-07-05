using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class UsuarioPerfilMap : ClassMap<UsuarioPerfil>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public UsuarioPerfilMap()
        {
            Table("TB_USUARIOPERFIL");
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioPerfil");
            References(x => x.Perfil, "ID_Perfil").Fetch.Join();
            References(x => x.Usuario, "ID_Usuario");
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
        }

    }

}