using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ModuloSolucaoEducacional : EntidadeBasica
    {
        public virtual Modulo Modulo { get; set; }
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual int Ordem { get; set; }

        public ModuloSolucaoEducacional()
        {
            this.Modulo = new Modulo();
            this.SolucaoEducacional = new SolucaoEducacional();
        }
    }
}
