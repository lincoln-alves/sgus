using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using NHibernate.Linq;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioCertificadoCertame : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<UsuarioCertificadoCertame> _repositorio;

        public BMUsuarioCertificadoCertame()
        {
            _repositorio = new RepositorioBase<UsuarioCertificadoCertame>();
        }

        public IQueryable<UsuarioCertificadoCertame> ObterTodos()
        {
            var query = _repositorio.session.Query<UsuarioCertificadoCertame>();
            return query.AsQueryable();
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }
    }
}