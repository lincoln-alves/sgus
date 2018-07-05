using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOManterTurmaProfessor
    {
        public virtual int IDTurmaProfessor { get; set; }
        public virtual string NomeProfessor { get; set; }
        public virtual string CPFProfessor { get; set; }
        public virtual string EmailProfessor { get; set; }
    }
}
