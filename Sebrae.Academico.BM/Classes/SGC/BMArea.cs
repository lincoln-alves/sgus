using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.SGC;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes.SGC
{
    public class BMArea : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Area> repositorio;

        public BMArea()
        {
            repositorio = new RepositorioBase<Area>();
        }

        public IQueryable<Area> ObterTodos()
        {
            return repositorio.session.Query<Area>();
        }

        public IQueryable<Area> ObterTodosPorUsuario(Usuario usuario)
        {
            // Obtém as áreas do usuário selecionado.
            var query = repositorio.session.Query<Area>().Where(x => x.CredenciadoArea.Any(c => c.CPF == usuario.CPF));

            // Filtra as subareas para exibir somente as subareas relacionadas com o usuário selecionado.
            query.ToList().ForEach(
                a => a.Subareas = a.Subareas.Where(s => s.CredenciadoArea.Any(x => x.CPF == usuario.CPF)).ToList());

            // Retorna uma consulta gloriosa.
            return query;
        }

        public Area ObterPorID(int id)
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
