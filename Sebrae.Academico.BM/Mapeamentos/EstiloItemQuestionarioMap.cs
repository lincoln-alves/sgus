using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class EstiloItemQuestionarioMap : ClassMap<EstiloItemQuestionario>
    {
        public EstiloItemQuestionarioMap()
        {
            Table("TB_EstiloItemQuestionario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_EstiloItemQuestionario");
            Map(x => x.Nome).Column("NM_Estilo").Not.Nullable();
            HasMany(x => x.ListaItemQuestionario).KeyColumn("ID_EstiloItemQuestionario");
        }
    }
}
