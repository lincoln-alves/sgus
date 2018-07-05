using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{

    public class BMHierarquiaNucleo : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<HierarquiaNucleo> repositorio;
        
        public BMHierarquiaNucleo()
        {
            repositorio = new RepositorioBase<HierarquiaNucleo>();
        }

        public HierarquiaNucleo ObterPorId(int idHierarquiaNucleo)
        {
            return repositorio.session.Query<HierarquiaNucleo>().FirstOrDefault(x => x.ID == idHierarquiaNucleo);
        }

        public IQueryable<HierarquiaNucleo> ObterTodos()
        {
            return repositorio.session.Query<HierarquiaNucleo>().AsQueryable();
        }

        public IQueryable<HierarquiaNucleo> ObterPorUf(Uf uf)
        {
            return repositorio.session.Query<HierarquiaNucleo>().Where(x => x.Uf == uf);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(HierarquiaNucleo model)
        {
            repositorio.Salvar(model);
        }

    }
}
