using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaFaq : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<TrilhaFaq> repositorio;

        #endregion

        public BMTrilhaFaq()
        {
            repositorio = new RepositorioBase<TrilhaFaq>();
        }

        public IQueryable<TrilhaFaq> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public IList<TrilhaFaq> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public TrilhaFaq ObterPorId(int pId)
        {
            var query = repositorio.session.Query<TrilhaFaq>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<TrilhaFaq> ObterPorFiltro(TrilhaFaq model)
        {
            var query = repositorio.session.Query<TrilhaFaq>();

            if (!string.IsNullOrEmpty(model.Nome))
            {
                query = query.Where(x => x.Nome.Contains(model.Nome));
            }

            return query as IList<TrilhaFaq>;
        }

        public void Salvar(TrilhaFaq model)
        {
            repositorio.Salvar(model);
        }

        public void Excluir(TrilhaFaq model)
        {
            repositorio.Excluir(model);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
