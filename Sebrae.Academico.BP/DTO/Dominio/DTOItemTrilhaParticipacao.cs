using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOItemTrilhaParticipacao : DTOEntidadeBasicaPorId
    {
        public virtual DTOUsuarioTrilha UsuarioTrilha { get; set; }
        public virtual string TextoParticipacao { get; set; }

        public virtual string NomeArquivoOriginal { get; set; }
        public virtual string TipoArquivoParticipacao { get; set; }
        public virtual string NomeArquivoServidor { get; set; }
        
        public virtual DateTime DataEnvio { get; set; }
        public virtual int TipoParticipacao { get; set; }
    }
}
