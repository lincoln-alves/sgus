using Sebrae.Academico.BP.DTO.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOResponderFormulario
    {
        public DTOResponderFormulario()
        {
            this.Respostas = new List<DTOCampo>();
            this.Situacao = new DTOSituacaoProcesso();
        }

        public virtual string inscricaoCPF { get; set; }
        public virtual int IdEtapa { get; set; }
        public virtual int IdEtapaResposta { get; set; }
        public virtual int IdAnalista { get; set; }
        public virtual int? IdCargo { get; set; }
        public virtual List<DTOCampo> Respostas { get; set; }
        public virtual DTOSituacaoProcesso Situacao { get; set; }
        public virtual List<DTOEtapaPermissaoNucleo> PermissoesNucleo { get; set; }
    }
}
