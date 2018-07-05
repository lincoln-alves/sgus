using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class CampoPorcentagem : EntidadeBasica
    {
        public virtual Campo Campo { get; set; }
        public virtual Campo CampoRelacionado { get; set; }
        public virtual DateTime UltimaAtualizacao { get; set; }
    }
}
