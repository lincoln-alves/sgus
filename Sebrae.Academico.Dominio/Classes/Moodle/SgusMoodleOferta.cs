using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class SgusMoodleOferta
    {
        public virtual int ID { get; set; }
        public virtual int CodigoCategoria { get; set; }
        public virtual int CodigoCurso { get; set; }
        public virtual string Nome { get; set; }
        public virtual DateTime DataCriacao { get; set; }
        public virtual DateTime DataAtualizacao { get; set; }
        public virtual int Desabilitado { get; set; }
    }
}
