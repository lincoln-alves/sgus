using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class JustificativaStatusMap : ClassMap<JustificativaStatus>
    {
        public JustificativaStatusMap()
        {
            Table("TB_JustificativaStatus");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_JustificativaStatus");
            Map(x => x.Descricao).Column("TX_Descricao").CustomSqlType("text").Not.Nullable();
            References(x => x.Turma).Column("ID_Turma").UniqueKey("IX_ID_Turma");
        }
    }
}
