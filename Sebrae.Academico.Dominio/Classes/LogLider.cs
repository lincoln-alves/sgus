using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogLider
    {
        public virtual int ID { get; set; }
        public virtual UsuarioTrilha Aluno { get; set; }
        public virtual UsuarioTrilha Lider { get; set; }
        public virtual TimeSpan? Tempo { get; set; }
        public virtual PontoSebrae PontoSebrae { get; set; }
    }
}