using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class ConfiguracaoPagamentoPublicoAlvoMap: ClassMap<ConfiguracaoPagamentoPublicoAlvo>
    {
        public ConfiguracaoPagamentoPublicoAlvoMap()
        {
            Table("TB_ConfiguracaoPagamentoPublicoAlvo");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_ConfiguracaoPagamentoPublicoAlvo");
            References(x => x.ConfiguracaoPagamento).Column("ID_ConfiguracaoPagamento");
            References(x => x.NivelOcupacional).Column("ID_NivelOcupacional");
            References(x => x.UF).Column("ID_UF");
            References(x => x.Perfil).Column("ID_Perfil");
            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
