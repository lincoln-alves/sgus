using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public sealed class TokenAcessoMap : ClassMap<TokenAcesso>
    {
        public TokenAcessoMap()
        {
            Table("TB_UsuarioTokenAcesso");
            Id(x => x.ID).GeneratedBy.Identity().Column("ID_UsuarioTokenAcesso");
            References(x => x.Usuario).Column("ID_Usuario");
            References(x => x.Fornecedor).Column("ID_Fornecedor");
            Map(x => x.Token).Column("IN_Token");
            Map(x => x.DataCriacao).Column("DT_Criacao");
            Map(x => x.IpAcesso).Column("IP_Acesso");
            Map(x => x.TokenMD5).Column("TokenMD5");
        }
    }
}
