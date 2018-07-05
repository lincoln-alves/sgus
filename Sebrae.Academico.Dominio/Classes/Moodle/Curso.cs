using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class Curso
    {
        public virtual int ID { get; set; }
        public virtual int CodigoCategoria { get; set; }
        public virtual string NomeCompleto { get; set; }
        public virtual string Nome { get; set; }
    }
}
