using System;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCertificadoCertame : BusinessManagerBase, IDisposable
    {
        private readonly RepositorioBase<CertificadoCertame> _repositorio;

        public BMCertificadoCertame()
        {
            _repositorio = new RepositorioBase<CertificadoCertame>();
        }

        public CertificadoCertame ObterPorId(int certificadoId)
        {
            return _repositorio.ObterPorID(certificadoId);
        }

        public IQueryable<CertificadoCertame> ObterTodos()
        {
            return _repositorio.ObterTodosIQueryable();
        }

        public void Dispose()
        {
            _repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(CertificadoCertame certificado)
        {
            _repositorio.Salvar(certificado);
        }
    }
}