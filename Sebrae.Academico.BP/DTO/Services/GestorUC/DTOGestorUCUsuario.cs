using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.GestorUC
{
    public class DTOGestorUCUsuario
    {
        public virtual string NomeUsuario { get; set; }
        public virtual string CPFUsuario { get; set; }
        public virtual string UF { get; set; }
        public virtual string NivelOcupacional { get; set; }
        public virtual int StatusMatriculaID { get; set; }
        public virtual string StatusMatricula { get; set; }
        public virtual DateTime DataSolicitacao { get; set; }
        public virtual int IdMatriculaOferta { get; set; }
        public virtual bool PermiteCancelamento { get; set; }
        public DTOTurmaCursosUsuarioPorCPF Turma { get; set; }

        public DTOGestorUCUsuario()
        {
            this.Turma = new DTOTurmaCursosUsuarioPorCPF();
        }
    }
}
