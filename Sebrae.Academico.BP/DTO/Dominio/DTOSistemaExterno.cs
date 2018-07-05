using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOSistemaExterno : DTOEntidadeBasica
    {
        public virtual string LinkAcesso { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool MesmaJanela { get; set; }
    }
}
