using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Modulo : EntidadeBasica
    {
        public virtual Capacitacao Capacitacao { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual CertificadoTemplate Certificado { get; set; }
        public virtual IList<ModuloSolucaoEducacional> ListaSolucaoEducacional { get; set; }
        public virtual IList<ModuloPreRequisito> ListaModuloPai { get; set; }
        public virtual IList<ModuloPreRequisito> ListaModuloFilho { get; set; }


        public Modulo()
        {
            this.Capacitacao = new Capacitacao();
            this.ListaSolucaoEducacional = new List<ModuloSolucaoEducacional>();
            this.ListaModuloPai = new List<ModuloPreRequisito>();
            this.ListaModuloFilho = new List<ModuloPreRequisito>();
        }
    }
}
