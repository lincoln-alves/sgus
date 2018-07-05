using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class CategoriaMoodle
    {
        public virtual Int64 ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string IdNumber { get; set; }
        
        public virtual string Descricao { get; set; }
        public virtual int DescricaoFormato { get; set; }                
    }
}
