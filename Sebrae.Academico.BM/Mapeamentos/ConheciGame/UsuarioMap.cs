using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.ConheciGame;

namespace Sebrae.Academico.BM.Mapeamentos.ConheciGame
{
    public class UsuarioConheciGameMap : ClassMap<UsuarioConheciGame>
    {
        public UsuarioConheciGameMap()
        {
            Table("tb_user");
            Id(x => x.ID).GeneratedBy.Identity().Column("user_id");
            Map(x => x.ID_UsuarioSebrae).Column("user_id_sebrae");
        }
    }
}
