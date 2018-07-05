using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class AlternativaMap : ClassMap<Alternativa>
    {
        public AlternativaMap()
        {
            Table("TB_Alternativa");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_Alternativa");
            References(x => x.Campo).Column("ID_Campo");
            Map(x => x.Nome).Column("NM_Alternativa");
            Map(x => x.Ordem).Column("VL_Ordem");
            Map(x => x.TipoCampo).Column("VL_Tipo");
            References(x => x.CampoVinculado).Column("ID_CampoVinculado");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
