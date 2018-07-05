using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOEfetuarInscricao
    {
        public DTOEfetuarInscricao()
        {
            this.Respostas = new List<DTOCampo>();
        }

        public virtual int IdEtapa { get; set; }
        public virtual string inscritoCPF { get; set; }
        public virtual List<DTOCampo> Respostas { get; set; }
    }
}
