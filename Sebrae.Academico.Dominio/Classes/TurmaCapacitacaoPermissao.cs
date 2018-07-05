using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TurmaCapacitacaoPermissao : EntidadeBasica
    {
        public virtual TurmaCapacitacao TurmaCapacitacao { get; set; }        
        public virtual Uf Uf { get; set; }
        public virtual int QuantidadeVagasPorEstado { get; set; }        
    }
}
