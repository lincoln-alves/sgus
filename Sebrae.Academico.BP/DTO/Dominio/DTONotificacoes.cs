using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTONotificacoes
    {
        public List<DTONotificacao> Notificacoes { get; set; }

        public int TotalNotificacoes { get; set; }
    }
}
