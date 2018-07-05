using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes.SGC;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes.SGC
{
    public class BMCredenciadoArea : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<CredenciadoArea> repositorio;

        public BMCredenciadoArea()
        {
            repositorio = new RepositorioBase<CredenciadoArea>();
        }

        public IQueryable<CredenciadoArea> ObterTodos()
        {
            return repositorio.session.Query<CredenciadoArea>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
