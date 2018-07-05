using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class SenhaSolicitacaoMap : ClassMap<SolicitacaoSenha>
    {
        public SenhaSolicitacaoMap()
        {
            Table("TB_SOLICITACAOSENHA");
            LazyLoad();
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_SolicitacaoSenha");
            References(x => x.Usuario).Column("ID_USUARIO").Not.Nullable().LazyLoad();
            Map(x => x.Token).Column("TX_TOKEN").Not.Nullable();
            Map(x => x.DataValidade).Column("DT_VALIDADE").Not.Nullable();
            Map(x => x.DataAlteracao, "DT_UltimaAtualizacao").Nullable();
            Map(x => x.UsuarioAlteracao, "NM_UsuarioAtualizacao").Nullable();
        }
    }
}