using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class LogMoodle
    {
        public virtual long ID { get; protected set; }
        public virtual long Tempo { get; set; }
        public virtual UsuarioMoodle Usuario { get; set; }
        public virtual string Ip { get; set; }
        public virtual Curso Curso { get; set; }
        public virtual string Acao { get; set; }
        public virtual string Url { get; set; }
        public virtual string Info { get; set; }
        public virtual string Module { get; set; }
    }
}
