using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CertificadoCertame
    {
        public virtual int ID { get; set; }
        public virtual int Ano { get; set; }
        public virtual string NomeCertificado { get; set; }
        public virtual FileServer Certificado { get; set; }
        public virtual IList<UsuarioCertificadoCertame> UsuariosCertificadosCertames { get; set; }
        public virtual DateTime? Data { get; set; }
    }
}