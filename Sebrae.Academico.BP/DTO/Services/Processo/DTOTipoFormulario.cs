using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOTipoFormulario
    {
        public virtual int ID { get; set; }
        public virtual string Tipo { get; set; }

        public DTOTipoFormulario()
        {

        }
    }
}
