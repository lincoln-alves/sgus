using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTODetalhamentoProcesso
    {
        public DTODetalhamentoProcesso()
        {
            Processo = new DTOProcessoInfo();
            Etapas = new List<DTOEtapaInfo>();
        }

        public virtual DTOProcessoInfo Processo { get; set; }
        public virtual List<DTOEtapaInfo> Etapas { get; set; }
    }
}
