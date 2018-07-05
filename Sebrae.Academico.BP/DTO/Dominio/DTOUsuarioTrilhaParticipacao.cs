using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    class DTOUsuarioTrilhaParticipacao : DTOEntidadeBasicaPorId
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string UF { get; set; }
        public virtual string NivelOcupacional { get; set; }
        public virtual string Trilha { get; set; }
        public virtual string StatusMatricula { get; set; }

    }
}
