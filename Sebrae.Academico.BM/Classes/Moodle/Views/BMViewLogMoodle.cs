using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes.Views;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Sebrae.Academico.BM.Classes.Moodle.Views
{
    public class BMViewLogMoodle : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<ViewLogMoodle> repositorio;
        public ISession session = NHibernateSessionManager.GetCurrentSessionMdl();


        public BMViewLogMoodle()
        {
            repositorio = new RepositorioBaseMdl<ViewLogMoodle>();
        }

        public IEnumerable<ViewLogMoodle> ObterTodos()
        {
            return session.Query<ViewLogMoodle>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
