using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SolucaoEducacionalUnidadeDemantesMap : ClassMap<SolucaoEducacionalUnidadeDemantes>
    {
        public SolucaoEducacionalUnidadeDemantesMap()
        {
            Table("TB_SolucaoEducacionalUnidadeDemantes");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalUnidadeDemantes");
            References(x => x.Cargo, "ID_Cargo");
            References(x => x.SolucaoEducacional, "ID_SolucaoEducacional");
            Map(x => x.DataAlteracao).Column("DT_Alteracao");
        }
    }
}
