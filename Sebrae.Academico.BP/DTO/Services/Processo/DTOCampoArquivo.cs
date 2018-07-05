using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOCampoArquivo
    {
        public virtual string NomeDoArquivoOriginal { get; set; }
        public virtual string TipoArquivo { get; set; }
        public virtual string NomeDoArquivoNoServidor { get; set; }
    }
}
