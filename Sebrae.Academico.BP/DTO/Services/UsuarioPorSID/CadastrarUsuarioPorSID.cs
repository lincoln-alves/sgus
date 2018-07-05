using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.UsuarioPorSID
{
    [Serializable]
    public class CadastraUsuarioPorSID : ConsultaUsuarioPorSID
    {
        public bool UsuarioRegistrado { get; set; }
    }
}
