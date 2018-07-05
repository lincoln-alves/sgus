using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.SGC;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes.SGC
{
    public class BMSubarea : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Subarea> repositorio;

        public BMSubarea()
        {
            repositorio = new RepositorioBase<Subarea>();
        }

        public IQueryable<Subarea> ObterTodos()
        {
            return repositorio.session.Query<Subarea>();
        }

        public IQueryable<Subarea> ObterTodosPorUsuario(Usuario usuario)
        {
            var query = repositorio.session.Query<Subarea>();

            return query.Where(x => x.CredenciadoArea.Any(c => c.CPF == usuario.CPF));
        }

        public Subarea ObterPorID(int id)
        {
            return repositorio.ObterPorID(id);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
