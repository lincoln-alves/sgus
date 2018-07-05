using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOConsultarStatusMatricula 
    {
        public List<DTOStatusMatricula> Lista { get; set; }

        public DTOConsultarStatusMatricula()
        {
            this.Lista = new List<DTOStatusMatricula>();
        }
    }
    
    [Serializable]
    public class DTOStatusMatricula
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
    }
}
