using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTipoItemTrilha : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<TipoItemTrilha> _repositorio;

        public BMTipoItemTrilha()
        {
            _repositorio = new RepositorioBase<TipoItemTrilha>();
        }

        public IQueryable<TipoItemTrilha> ObterTodos()
        {
            return _repositorio.ObterTodosIQueryable();
        }

        public TipoItemTrilha ObterPorId(int id)
        {
            return _repositorio.ObterTodosIQueryable().FirstOrDefault(x => x.ID == id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(_repositorio);
            GC.Collect();
        }
    }
}