using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class SolucaoEducacionalPreRequisitoMap : ClassMap<SolucaoEducacionalPreRequisito>
    {
        public SolucaoEducacionalPreRequisitoMap()
        {
            Table("TB_SolucaoEducacionalPreRequisito");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalPreRequisito");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional");
            References(x => x.PreRequisito).Column("ID_SolucaoEducacionaPreRequisito");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
        }
    }
}
