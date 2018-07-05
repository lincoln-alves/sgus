using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOCertificadosPorNaoCertificados
    {
        public virtual int Certificado { get; set; }
        public virtual int NaoCertificado { get; set; }
    }
}
