using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.ConheciGame;

namespace Sebrae.Academico.BM.Mapeamentos.ConheciGame
{
    public class ConteudoMap : ClassMap<Conteudo>
    {
        public ConteudoMap()
        {
            Table("tb_category");
            Id(x => x.ID).GeneratedBy.Identity().Column("category_id");
            Map(x => x.Nome).Column("category_name");
            Map(x => x.Ativo).Column("active");
        }
    }
}
