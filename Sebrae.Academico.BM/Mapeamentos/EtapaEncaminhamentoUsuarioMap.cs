using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class EtapaEncaminhamentoUsuarioMap : ClassMap<EtapaEncaminhamentoUsuario>
    {
        public EtapaEncaminhamentoUsuarioMap()
        {
            Table("TB_EtapaEncaminhamentoUsuario");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_EtapaEncaminhamentoUsuario");
            References(x => x.EtapaResposta).Column("ID_EtapaResposta");
            References(x => x.EtapaPermissaoNucleo).Column("ID_EtapaPermissaoNucleo");
            References(x => x.UsuarioEncaminhamento).Column("ID_UsuarioEncaminhamento");

            Map(x => x.StatusEncaminhamento).Column("IN_StatusEncaminhamento");
            Map(x => x.DataSolicitacaoEncaminhamento).Column("DT_SolicitacaoEncaminhamento");
            Map(x => x.Justificativa).Column("TX_Justificativa");

            Map(x => x.DataAlteracao).Column("DT_UltimaAtualizacao");
            Map(x => x.UsuarioAlteracao).Column("NM_UsuarioAtualizacao");
        }
    }
}
