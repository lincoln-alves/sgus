using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class AvaliacaoMap : ClassMap<Avaliacao>
    {
        public AvaliacaoMap()
        {
            Table("TB_Avaliacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Avaliacao");
            
            Map(x => x.Status, "VL_Status").CustomType<enumStatusAvaliacao>();
            
            References(x => x.Analista).Column("ID_Usuario");
            References(x => x.Turma).Column("ID_Turma");

            HasManyToMany(x => x.Respostas)
                .Table("TB_AvaliacaoQuestaoResposta")
                .ParentKeyColumn("ID_Avaliacao")
                .ChildKeyColumn("ID_QuestaoResposta");
        }
    }
}