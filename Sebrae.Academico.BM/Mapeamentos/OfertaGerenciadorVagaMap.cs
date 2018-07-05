using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class OfertaGerenciadorVagaMap : ClassMap<OfertaGerenciadorVaga>
    {
        public OfertaGerenciadorVagaMap()
        {
            Table("TB_OfertaGerenciadorVaga");
            LazyLoad();
            Id(x => x.ID, "ID_OfertaGerenciadorVaga").GeneratedBy.Identity();
            References(x => x.Oferta).Column("ID_Oferta");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.UF).Column("ID_UF");
            Map(x => x.Contemplado, "IN_Contemplado");
            Map(x => x.Vigente, "IN_Vigente");
            Map(x => x.DataSolicitacao, "DT_Solicitacao");
            Map(x => x.DataAlteracao, "DT_AtualizacaoCadastral");
            Map(X => X.Descricao, "TX_Observacao");
            Map(x => x.Resposta, "TX_Resposta");
            Map(x => x.VagasAnteriores, "IN_VagasAnteriores");
            Map(x => x.VagasRecusadas, "IN_VagasRecusadas");
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao");
          }
    }
}
