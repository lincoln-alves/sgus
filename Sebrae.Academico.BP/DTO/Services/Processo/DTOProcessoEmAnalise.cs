using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOProcessoEmAnalise
    {
        public DTOProcessoEmAnalise()
        {
            Situacao = new DTOSituacaoProcesso();
        }

        public virtual string Responsavel { get; set; }
        public virtual string Unidade { get; set; }
        public virtual string BeneficiariosComCapacitacao { get; set; }
        public virtual DTOSituacaoProcesso Situacao { get; set; }
    }
}
