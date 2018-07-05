using Sebrae.Academico.BP.DTO.Dominio;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.Protocolo
{
    public class RetornoProtocolo
    {
        public List<DTOProtocolo> Protocolos { get; set; }

        public List<DTOUsuario> UsuariosDestinatarios { get; set; }

        public DTOUsuario UsuarioRemetente { get; set; }

        public string MensagemRetorno { get; set; }

        // Mensagem de erro ou não no cadastro.
        public bool ApresentarErro { get; set; }
    }
}
