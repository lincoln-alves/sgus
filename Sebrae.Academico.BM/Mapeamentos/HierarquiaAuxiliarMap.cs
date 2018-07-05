using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class HierarquiaAuxiliarMap : ClassMap<HierarquiaAuxiliar>
    {
        public HierarquiaAuxiliarMap()
        {
            Table("TB_HierarquiaAuxiliar");
            LazyLoad();
            Id(x => x.ID, "ID_HierarquiaAuxiliar").GeneratedBy.Identity();
            Map(x => x.CodUnidade, "NM_HierarquiaCodUnidade").Not.Nullable();

            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");

            References(x => x.Usuario).Column("ID_Usuario").Not.Nullable();                       
        }
    }
}