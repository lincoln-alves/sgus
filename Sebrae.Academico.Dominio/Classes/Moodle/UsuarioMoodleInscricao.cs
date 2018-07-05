using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class UsuarioMoodleInscricao
    {
        public virtual Int64 ID { get; set; }
        public virtual Int64 Status { get; set; }
        public virtual Int64 IDInscricao { get; set; }
        
        public virtual Int64 TempoInicio { get; set; }
        public virtual Int64 TempoFim { get; set; }
        public virtual Int64 IDModificador { get; set; }
        public virtual Int64 TempoCriacao { get; set; }
        public virtual Int64 TempoModificacao { get; set; }

        public virtual Inscricao Inscricao { get; set; }
        public virtual UsuarioMoodle UsuarioMoodle { get; set; }

        public UsuarioMoodleInscricao()
        {
            this.Status = 0;
            this.TempoInicio = 0;
            this.TempoFim = 0;
            this.IDModificador = 2;
            this.TempoCriacao = 0;
            this.TempoModificacao = 0;
        }
    }
}
