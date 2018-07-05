using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.ConheciGame
{
    public class DTOParticipacaoConheciGame
    {
        public int ID_UsuarioTrilha { get; set; }
        public int ID_ItemTrilha { get; set; }
        public int QuantidadeAcertos { get; set; }
    }
}
