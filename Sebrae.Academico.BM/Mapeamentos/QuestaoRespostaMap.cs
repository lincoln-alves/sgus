using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestaoRespostaMap : ClassMap<QuestaoResposta>
    {
        public QuestaoRespostaMap()
        {
            Table("TB_QuestaoResposta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_QuestaoResposta");

            Map(x => x.Dominio, "VL_Dominio").CustomType<enumDominio>();
            Map(x => x.Comentario).Column("TX_Comentario");

            References(x => x.MatriculaTurma).Column("ID_MatriculaTurma");
            References(x => x.Questao).Column("ID_Questao");
            References(x => x.StatusMatricula).Column("ID_StatusMatricula");

            HasManyToMany(x => x.Avaliacoes)
                .Table("TB_AvaliacaoQuestaoResposta")
                .ParentKeyColumn("ID_QuestaoResposta")
                .ChildKeyColumn("ID_Avaliacao");
        }
    }
}