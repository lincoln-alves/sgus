using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class AlternativaResposta : EntidadeBasica
    {
        public virtual CampoResposta CampoResposta { get; set; }
        public virtual Alternativa Alternativa { get; set; }

        public AlternativaResposta()
        {
            this.CampoResposta = new CampoResposta();
            //this.Alternativa = new Alternativa();
        }
    }
}
