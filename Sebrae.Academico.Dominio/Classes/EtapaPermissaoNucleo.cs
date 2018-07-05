using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EtapaPermissaoNucleo : EntidadeBasica
    {
        public virtual HierarquiaNucleoUsuario HierarquiaNucleoUsuario { get; set; }
        public virtual Etapa Etapa { get; set; }

        public virtual IList<EtapaRespostaPermissao> PermissoesNucleoEtapaResposta { get; set; }
    }
}
