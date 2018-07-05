using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOTurmaCapacitacao : DTOEntidadeBasica
    {
        public virtual DTOCapacitacao Capacitacao { get; set; }
    }
}
