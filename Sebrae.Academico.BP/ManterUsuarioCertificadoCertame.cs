using System;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioCertificadoCertame : RepositorioBase<UsuarioCertificadoCertame>, IDisposable
    {
        //private readonly BMUsuarioCertificadoCertame _bmUsuarioCertificadoCertame;

        //public ManterUsuarioCertificadoCertame()
        //{
        //    _bmUsuarioCertificadoCertame = new BMUsuarioCertificadoCertame();
        //}

        public List<UsuarioCertificadoCertame> ObterCertamesPorUsuario(Usuario usuario)
        {
            var listaCertificado = ObterTodos().Where(x => x.Usuario.ID == usuario.ID).OrderByDescending(x => x.CertificadoCertame.Ano).ToList();

            return listaCertificado;
        }

        public UsuarioCertificadoCertame ObterCertamePorUsuarioCertificado(Usuario usuario, CertificadoCertame certificado)
        {
            return ObterTodos().FirstOrDefault(x => x.Usuario.ID == usuario.ID && x.CertificadoCertame.ID == certificado.ID);
        }

        public void Dispose()
        {
            base.Dispose();
        }
    }
}
