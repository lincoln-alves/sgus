using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.UsuarioPorSID
{
    [Serializable]
    public class DTOUsuarioRecuperado
    {
        public string Nome {get;set;}
        public string Email { get; set; }
    }
}
