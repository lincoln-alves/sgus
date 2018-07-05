using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TipoQuestionarioMap: ClassMap<TipoQuestionario>
    {
        public TipoQuestionarioMap()
        {
            Table("TB_TipoQuestionario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Assigned().Column("ID_TipoQuestionario");
            Map(x => x.Nome).Column("NM_TipoQuestionario").Not.Nullable();
            HasMany(x => x.ListaQuestionario).KeyColumn("ID_TipoQuestionario");
        }
    }
}
