using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogConsultorEducacional
    {
        public virtual int ID { get; set; }
        public virtual Usuario ConsultorEducacional { get; set; }
        public virtual Usuario UsuarioAlteracao { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual DateTime Data { get; set; }
    }
}
