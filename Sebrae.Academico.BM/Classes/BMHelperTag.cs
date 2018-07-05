using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMHelperTag : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<HelperTag> _repositorio;

        public BMHelperTag()
        {
            _repositorio = new RepositorioBase<HelperTag>();
        }

        public void Salvar(HelperTag helper)
        {
            _repositorio.Salvar(helper);
        }

        public IQueryable<HelperTag> ObterTodos()
        {
            return _repositorio.ObterTodosIQueryable();
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public HelperTag ObterPorId(int id)
        {
            return _repositorio.ObterPorID(id);
        }

        public HelperTag ObterPorChavePagina(string chave, int paginaId)
        {
            return
                _repositorio.session.Query<HelperTag>().FirstOrDefault(x => x.Chave == chave && x.Pagina.ID == paginaId);
        }
    }
}
