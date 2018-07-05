using System;

namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public class ItemTrilhaParticipacao: EntidadeBasicaPorId
    {
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual ItemTrilha ItemTrilha { get; set; }

        public virtual string ArquivoParticipacao { get; set; }
        public virtual string TextoParticipacao { get; set; }
        public virtual DateTime DataParticipacao { get; set; }


    }
}
