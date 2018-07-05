using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Classes.Views;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Sebrae.Academico.BM.Classes.PortalUC
{
    public class BMLogAcoesPortal : BusinessManagerBase, IDisposable
    {
        public ISession session = NHibernateSessionManager.GetCurrentSessionPortal();

        public BMLogAcoesPortal()
        {
        }

        public IQueryable<LogAcoesPortal> ObterTodos()
        {
            return session.Query<LogAcoesPortal>();
        }

        public void Dispose()
        {
            session.Connection.Close(); 
        }

        public void Salvar(LogAcoesPortal logAcoesPortal)
        {
            session.Persist(logAcoesPortal);
        }
    }
}
