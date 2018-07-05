using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TipoItemQuestionarioMap: ClassMap<TipoItemQuestionario>
    {
        public TipoItemQuestionarioMap()
        {
            Table("TB_TipoItemQuestionario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_TipoItemQuestionario");
            Map(x => x.Nome).Column("NM_TipoItemQuestionario");
            Map(x => x.TodosEstilos).Column("VL_TodosEstilos");

            HasManyToMany(a => a.ListaEstilosItemQuestionario)
                .Table("TB_TipoItemQuestionario_EstiloItemQuestionario")
                .ParentKeyColumn("ID_TipoItemQuestionario")
                .ChildKeyColumn("ID_EstiloItemQuestionario");
        }
    }
}