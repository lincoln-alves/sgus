using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewUsuarioTrilhaAtividadeFormativaParticipacao
    {
        public virtual Int64 Linha { get; set; }
        public virtual Trilha TrilhaOrigem { get; set; }
        public virtual TrilhaNivel TrilhaNivelOrigem { get; set; }
        public virtual TrilhaTopicoTematico TopicoTematico { get; set; }
        public virtual Usuario UsuarioOrigem { get; set; }
        public virtual string TemParticipacao { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual int IdTrilhaAtividadeFormativaParticipacao { get; set; }
        
    }
}
