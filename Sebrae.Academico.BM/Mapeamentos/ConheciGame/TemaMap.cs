using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.ConheciGame;

namespace Sebrae.Academico.BM.Mapeamentos.ConheciGame
{
    public class TemaMap : ClassMap<Tema>
    {
        public TemaMap()
        {
            Table("tb_theme");
            Id(x => x.ID).GeneratedBy.Identity().Column("theme_id");
            Map(x => x.Nome).Column("theme_name");
            Map(x => x.Ativo).Column("active");
            References(x => x.Conteudo).Column("category_id");
        }
    }
}
