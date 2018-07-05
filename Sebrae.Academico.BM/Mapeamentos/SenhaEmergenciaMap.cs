using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SenhaEmergenciaMap : ClassMap<SenhaEmergencia>
    {
        public SenhaEmergenciaMap()
        {
            Table("TB_SENHAEMERGENCIA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SENHAEMERGENCIA");
            Map(x => x.Senha).Column("TX_SENHA").Not.Nullable();
            Map(x => x.DataValidade).Column("DT_VALIDADE").Not.Nullable();
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
            References(x => x.UF).Column("ID_UF");
        }

    }

}