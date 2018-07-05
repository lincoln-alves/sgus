using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalPreRequisito : EntidadeBasica
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual SolucaoEducacional PreRequisito { get; set; }

        public SolucaoEducacionalPreRequisito()
        {
            this.SolucaoEducacional = new SolucaoEducacional();
            this.PreRequisito = new SolucaoEducacional();
        }
    }
}
