using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class UsuarioCargoMap : ClassMap<UsuarioCargo>
    {
        public UsuarioCargoMap()
        {
            Table("TB_UsuarioCargo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioCargo");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.Cargo).Column("ID_Cargo");
        }
    }
}