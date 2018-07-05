using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOEstatisticaHomePortal
    {
        public virtual int ano { get; set; }
        public virtual int usuarios { get; set; }
        public virtual int ofertas { get; set; }        
        public virtual int certificados { get; set; }
        public virtual int horas { get; set; }
    }
}
