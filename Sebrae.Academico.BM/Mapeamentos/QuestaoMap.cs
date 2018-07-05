using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class QuestaoMap : ClassMap<Questao>
    {
        public QuestaoMap()
        {
            Table("TB_Questao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Questao");

            Map(x => x.Titulo).Column("TX_Titulo");
            Map(x => x.Pergunta).Column("TX_Questao");
            Map(x => x.Tipo, "VL_Tipo").CustomType<enumTipoQuestao>();
            Map(x => x.Ordem).Column("VL_Ordem");

            HasMany(x => x.Respostas).KeyColumn("ID_Questao").Cascade.All();
        }
    }
}