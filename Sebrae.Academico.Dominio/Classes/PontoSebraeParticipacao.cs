using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class PontoSebraeParticipacao : EntidadeBasicaPorId
    {
        public virtual PontoSebrae PontoSebrae { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual DateTime PrimeiraParticipacao { get; set; }
        public virtual DateTime? UltimaParticipacao { get; set; }
    }
}
