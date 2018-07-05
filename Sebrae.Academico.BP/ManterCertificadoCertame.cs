using System;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterCertificadoCertame : BusinessProcessBase, IDisposable
    {
        private readonly BMCertificadoCertame _bmCertificadoCertame;

        public ManterCertificadoCertame()
        {
            _bmCertificadoCertame = new BMCertificadoCertame();
        }

        public CertificadoCertame ObterPorId(int id)
        {
            return _bmCertificadoCertame.ObterPorId(id);
        }

        public void Dispose()
        {
            _bmCertificadoCertame.Dispose();
        }

        public void Salvar(CertificadoCertame certificado)
        {
            _bmCertificadoCertame.Salvar(certificado);
        }
    }
}
