using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CampoMetaValue : EntidadeBasica
    {
        public virtual Campo Campo { get; set; }
        public virtual CampoMeta CampoMeta { get; set; }        
        public virtual string MetaValue { get; set; }
    }
}
