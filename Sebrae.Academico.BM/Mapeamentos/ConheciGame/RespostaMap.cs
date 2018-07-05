using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes.ConheciGame;

namespace Sebrae.Academico.BM.Mapeamentos.ConheciGame
{
    public class RespostaMap : ClassMap<Resposta>
    {
        public RespostaMap()
        {
            Table("tb_match_answer");
            Id(x => x.ID).GeneratedBy.Identity().Column("match_answer_id");
            References(x => x.Usuario).Column("user_id");
            References(x => x.Partida).Column("match_id");
            Map(x => x.Acertou).Column("match_answer_result");
        }
    }
}
