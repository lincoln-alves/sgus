using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOAcompanharProcessosSolicitados : DTOAnalisarEtapasProcesso
    {
        public virtual DTOSituacaoProcesso Situacao { get; set; }
    }
}
