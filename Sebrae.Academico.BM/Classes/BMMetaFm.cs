using System;
using System.Collections.Generic;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMetaFm : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<MetaFm> _repositorio;

        public BMMetaFm()
        {
            _repositorio = new RepositorioBase<MetaFm>();
        }

        public IEnumerable<MetaFm> ObterTodos()
        {
            return _repositorio.session.Query<MetaFm>();
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}
