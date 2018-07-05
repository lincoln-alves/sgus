using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.ConheciGame;

namespace Sebrae.Academico.BM.Mapeamentos.ConheciGame
{
    public class PartidaMap : ClassMap<Partida>
    {
        public PartidaMap()
        {
            Table("tb_match");
            Id(x => x.ID).GeneratedBy.Identity().Column("id");
            References(x => x.Usuario).Column("invitation_user");
            References(x => x.Tema).Column("theme_id");
        }
    }
}
