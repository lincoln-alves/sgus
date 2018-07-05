using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TurmaCapacitacaoMap : ClassMap<TurmaCapacitacao>
    {
        public TurmaCapacitacaoMap()
        {
            Table("TB_TurmaCapacitacao");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_TurmaCapacitacao");
            Map(x => x.Nome).Column("NM_TurmaCapacitacao");
            Map(x => x.DataInicio).Column("DT_Inicio").Nullable();
            Map(x => x.DataFim).Column("DT_Fim").Nullable();
            References(x => x.Capacitacao).Column("ID_Capacitacao");

            HasMany(x => x.ListaPermissao).KeyColumn("ID_TurmaCapacitacao").AsBag().Inverse()
                .LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaQuestionarioAssociacao).KeyColumn("ID_TurmaCapacitacao").NotFound.Ignore().AsBag().Inverse()
                .LazyLoad().Cascade.All();
        }
    }
}
