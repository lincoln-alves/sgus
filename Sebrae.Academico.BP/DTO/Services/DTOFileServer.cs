using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOFileServer
    {
        public virtual string NomeDoArquivoOriginal { get; set; }
        public virtual string TipoArquivo { get; set; }
        public virtual string NomeDoArquivoNoServidor { get; set; }
        public virtual int? IdFileServer { get; set; }
        public virtual string FileServerLink { get; set; }
        public virtual bool? MediaServer { get; set; }
        public virtual string DataEnvio { get; set; }

        public string Usuario { get; set; }
    }
}
