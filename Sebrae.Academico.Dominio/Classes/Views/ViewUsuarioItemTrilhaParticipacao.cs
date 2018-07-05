using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewUsuarioItemTrilhaParticipacao
    {
        public virtual Int64 Linha { get; set; }
        public virtual Trilha TrilhaOrigem { get; set; }
        public virtual TrilhaNivel TrilhaNivelOrigem { get; set; }
        public virtual TrilhaTopicoTematico TopicoTematico { get; set; }
        public virtual Usuario UsuarioOrigem { get; set; }
        public virtual ItemTrilha ItemTrilha { get; set; }
        public virtual string TemParticipacao { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual int IdItemTrilhaParticipacao { get; set; }
        
    }

}
