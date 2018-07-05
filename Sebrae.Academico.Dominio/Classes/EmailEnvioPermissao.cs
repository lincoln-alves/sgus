using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EmailEnvioPermissao : EntidadeBasica
    {
        public virtual EmailEnvio EmailEnvio { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual StatusMatricula Status { get; set; }
    }
}
