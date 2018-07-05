using System;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Views
{

    public class BMViewSolicitacaoDemanda : BusinessManagerBase
    {
        private RepositorioBase<ViewSolicitacaoDemanda> repositorio;

        public BMViewSolicitacaoDemanda()
        {
            repositorio = new RepositorioBase<ViewSolicitacaoDemanda>();
        }

        public IQueryable<ViewSolicitacaoDemanda> ObterTodos()
        {
            return repositorio.session.Query<ViewSolicitacaoDemanda>();
        }
    }
}
