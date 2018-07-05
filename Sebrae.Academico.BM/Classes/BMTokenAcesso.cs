using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Web;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.BM.Classes
{
    public class BMTokenAcesso : BusinessManagerBase, IDisposable
    {
        public RepositorioBase<TokenAcesso> repositorio;

        protected int periodoDeValidadadeDoTokenEmHoras = 24;

        public BMTokenAcesso()
        {
            repositorio = new RepositorioBase<TokenAcesso>();
        }

        public void Salvar(TokenAcesso pTokenAcesso)
        {
            repositorio.Salvar(pTokenAcesso);
        }

        public TokenAcesso ObterTokenValido(Guid token)
        {
            return repositorio.session.Query<TokenAcesso>().FirstOrDefault(x => x.Token == token && x.DataCriacao >= DateTime.Now.AddHours((-1 * periodoDeValidadadeDoTokenEmHoras)));
        }

        public TokenAcesso ObterTokenValidoMd5TokenAcesso(string token)
        {
            return repositorio.session.Query<TokenAcesso>().FirstOrDefault(x => x.TokenMD5 == token && x.DataCriacao >= DateTime.Now.AddHours((-1 * periodoDeValidadadeDoTokenEmHoras)));
        }

        public TokenAcesso ObterTokenValidoPorUsuarioFornecedor(Usuario usuario, Fornecedor fornecedor)
        {
            return repositorio.session.Query<TokenAcesso>().FirstOrDefault(x => x.Usuario == usuario && x.Fornecedor == fornecedor && x.DataCriacao >= DateTime.Now.AddHours((-1 * periodoDeValidadadeDoTokenEmHoras))); 
        }

        public void Dispose()
        {
            GC.SuppressFinalize(repositorio);
        }
    }
}
