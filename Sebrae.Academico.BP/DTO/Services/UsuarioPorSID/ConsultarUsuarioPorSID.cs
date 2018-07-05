using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.UsuarioPorSID
{
    [Serializable]
    public class ConsultaUsuarioPorSID
    {
        public bool UsuarioEncontrado { get; set; }
        public string Mensagem { get; set; }
        public DTOUsuarioRecuperado Usuario { get; set; }

        public ConsultaUsuarioPorSID()
        {
            Usuario = new DTOUsuarioRecuperado();
        }
    }
}
