using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BM.Classes
{
    public class BMAssuntoTrilhaFaq : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<AssuntoTrilhaFaq> repositorio;

        #endregion

        public BMAssuntoTrilhaFaq()
        {
            repositorio = new RepositorioBase<AssuntoTrilhaFaq>();
        }
        
        public IQueryable<AssuntoTrilhaFaq> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public IList<AssuntoTrilhaFaq> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public AssuntoTrilhaFaq ObterPorId(int pId)
        {
            var query = repositorio.session.Query<AssuntoTrilhaFaq>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<AssuntoTrilhaFaq> ObterPorFiltro(AssuntoTrilhaFaq model)
        {
            var query = repositorio.session.Query<AssuntoTrilhaFaq>();

            if (!string.IsNullOrEmpty(model.Nome))
            {
                query = query.Where(x => x.Nome.Contains(model.Nome));
            }

            return query as IList<AssuntoTrilhaFaq>;
        }

        public void Salvar(AssuntoTrilhaFaq model)
        {
            repositorio.Salvar(model);
        }

        public void Excluir(AssuntoTrilhaFaq model)
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
