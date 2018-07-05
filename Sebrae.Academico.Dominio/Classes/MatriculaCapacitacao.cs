using System;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Collections;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{

    public class MatriculaCapacitacao : EntidadeBasicaComStatus
    {
        public MatriculaCapacitacao()
        {
            Capacitacao = new Capacitacao();
            ListaMatriculaTurmaCapacitacao = new List<MatriculaTurmaCapacitacao>();
        }

        public virtual Capacitacao Capacitacao { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Uf UF { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual string CDCertificado { get; set; }
        public virtual DateTime? DataGeracaoCertificado { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual int? CertificadoEmitidoPorGestor { get; set; }
        public virtual IList<MatriculaTurmaCapacitacao> ListaMatriculaTurmaCapacitacao { get; set; }
    }


}
