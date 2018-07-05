using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class AlternativaRespostaMap : ClassMap<AlternativaResposta>
    {
        public AlternativaRespostaMap()
        {
            Table("TB_AlternativaResposta");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_AlternativaResposta");
            References(x => x.CampoResposta).Column("ID_CampoResposta").Cascade.None();
            References(x => x.Alternativa).Column("ID_Alternativa").Cascade.None();

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
