using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SolucaoEducacionalExtraCurricularMap : ClassMap<SolucaoEducacionalExtraCurricular>
    {
        public SolucaoEducacionalExtraCurricularMap()
        {
            Table("TB_SolucaoEducacionalExtraCurricular");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalExtraCurricular");
            References(x => x.FormaAquisicao).Column("ID_FormaAquisicao");
            Map(x => x.Nome).Column("NM_SolucaoEducacionalExtraCurricular");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
