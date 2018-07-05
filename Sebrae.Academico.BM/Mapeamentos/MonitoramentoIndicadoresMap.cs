using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class MonitoramentoIndicadoresMap : ClassMap<MonitoramentoIndicadores>
    {
        /// <summary>
        /// Construtor.
        /// </summary>
        public MonitoramentoIndicadoresMap()
        {
            Table("TB_MonitoramentoIndicadores");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_MonitoramentoIndicadores");
            Map(x => x.Ano).Column("ANO");
            HasMany(x => x.ListaValores).KeyColumn("ID_MonitoramentoIndicadores").AsBag().Inverse().Cascade.AllDeleteOrphan(); 
        }

    }
}