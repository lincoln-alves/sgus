using Sebrae.Academico.Dominio.Classes;
using FluentNHibernate.Mapping;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class TurmaProfessorMap : ClassMap<TurmaProfessor>
    {
        public TurmaProfessorMap()
        {
            Table("TB_TurmaProfessor");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TurmaProfessor");
            References(x => x.Turma).Column("ID_Turma").Cascade.None().Not.Nullable().UniqueKey("UK_TURMA_PROFESSOR");
            References(x => x.Professor).Column("ID_Usuario").Cascade.None().Not.Nullable().UniqueKey("UK_TURMA_PROFESSOR");
        }
    }
}
