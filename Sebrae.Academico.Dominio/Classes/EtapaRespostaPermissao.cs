using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EtapaRespostaPermissao : EntidadeBasicaPorId
    {
        public virtual EtapaResposta EtapaResposta { get; set; }
        public virtual EtapaPermissaoNucleo EtapaPermissaoNucleo { get; set; }
    }
}
