using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOAnalisarEtapasProcesso
    {
        public DTOAnalisarEtapasProcesso()
        {
            Processo = new DTOProcessoInfo();
            ListaEtapas = new List<DTOEtapa>();
        }
        
        public virtual DTOProcessoInfo Processo { get; set; }
        public virtual List<DTOEtapa> ListaEtapas { get; set; }
    }
}
