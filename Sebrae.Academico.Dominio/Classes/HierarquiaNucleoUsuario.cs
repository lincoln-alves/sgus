using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class HierarquiaNucleoUsuario : EntidadeBasica
    {
        public virtual HierarquiaNucleo HierarquiaNucleo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual bool IsGestor { get; set; }

        public virtual List<EtapaRespostaPermissao> PermissoesEtapaResposta { get; set; }
    }
}
