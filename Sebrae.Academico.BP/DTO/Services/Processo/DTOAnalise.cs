using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOAnalise
    {
        public virtual int IdEtapa { get; set; }
        public virtual string Analise { get; set; }
        public virtual bool? Aprovar { get; set; }
    }
}
