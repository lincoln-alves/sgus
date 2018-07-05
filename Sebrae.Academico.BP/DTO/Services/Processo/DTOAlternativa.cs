using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOAlternativa
    {
        public DTOAlternativa()
        {
            TipoCampo = new DTOTipoFormulario();
        }
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DTOTipoFormulario TipoCampo { get; set; }
        public virtual string Reposta { get; set; }
        public virtual bool OpcaoRespondida { get; set; }
        public virtual int? ID_CampoVinculado { get; set; }
    }
}
