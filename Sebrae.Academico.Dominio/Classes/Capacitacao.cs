using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Capacitacao : EntidadeBasica
    {
        public virtual Programa Programa { get; set; }
        public virtual IList<Modulo> ListaModulos { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual DateTime? DataInicioInscricao { get; set; }
        public virtual DateTime? DataFimInscricao { get; set; }
        public virtual CertificadoTemplate Certificado { get; set; }
        public virtual bool PermiteCancelarMatricula { get; set; }
        public virtual bool PermiteAlterarSituacao { get; set; }
        public virtual bool PermiteMatriculaPeloGestor { get; set; }
        public virtual IList<TurmaCapacitacao> ListaTurmas { get; set; }
        public virtual int? IdNodePortal { get; set; }

        public Capacitacao()
        {
            this.Programa = new Programa();
            this.Certificado = new CertificadoTemplate();
            ListaModulos = new List<Modulo>();
            ListaTurmas = new List<TurmaCapacitacao>();
        }
    }
}
