using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{

    public class BMHierarquia : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Hierarquia> repositorio;
        
        public BMHierarquia()
        {
            repositorio = new RepositorioBase<Hierarquia>();
        }

        public Hierarquia ObterPorEmail(string email)
        {
            return repositorio.session.Query<Hierarquia>().FirstOrDefault(x => x.Email == email);
        }

        public Hierarquia ObterDiretor(string unidade)
        {
            return repositorio.session.Query<Hierarquia>().FirstOrDefault(x => x.CodUnidade.Length == 5 && unidade.StartsWith(x.CodUnidade));
        }

        public string ObterNomeUnidadePorCodUnidade(string unidade)
        {
            return repositorio.session.Query<Hierarquia>().Where(x => x.CodUnidade == unidade).FirstOrDefault().Unidade;
        }

        public IList<Hierarquia> ObterDiretorias()
        {
            return repositorio.session.Query<Hierarquia>().Where(x => x.CodUnidade.Length == 5).GroupBy(x => new { x.CodUnidade, x.Unidade }).Select(g => new Hierarquia { CodUnidade = g.Key.CodUnidade, Unidade = g.Key.Unidade }).ToList();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
