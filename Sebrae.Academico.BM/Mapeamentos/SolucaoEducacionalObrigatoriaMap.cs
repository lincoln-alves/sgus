using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class SolucaoEducacionalObrigatoriaMap : ClassMap<SolucaoEducacionalObrigatoria>
    {
        public SolucaoEducacionalObrigatoriaMap()
        {
            Table("TB_SolucaoEducacionalObrigatoria");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolucaoEducacionalObrigatoria");
            References(x => x.SolucaoEducacional).Column("ID_SolucaoEducacional");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            Map(x => x.DataAlteracao).Column("DT_ULTIMAATUALIZACAO");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
