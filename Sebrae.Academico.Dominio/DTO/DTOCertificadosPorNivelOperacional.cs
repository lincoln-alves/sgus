using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOCertificadosPorNivelOperacional
    {
        public virtual string Nome { get; set; }
        public virtual int Certificados { get; set; }
        public virtual int Quantidade { get; set; }
    }
}
