using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Classes.Moodle
{
    public class BMCategoriaMoodle : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<CategoriaMoodle> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMCategoriaMoodle()
        {
            repositorio = new RepositorioBaseMdl<CategoriaMoodle>();
        }

        public IList<CategoriaMoodle> ObterTodos()
        {
            var query = repositorio.session.Query<CategoriaMoodle>();
            return query.ToList<CategoriaMoodle>();
        }

        public IQueryable<CategoriaMoodle> ObterTodosIQueryable()
        {
            var query = repositorio.session.Query<CategoriaMoodle>();
            return query;
        }
    }
}
