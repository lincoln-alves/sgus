using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Moodle
{
    public class Inscricao
    {
        public virtual int ID { get; set; }
        public virtual int IDCurso { get; set; }

        /// <summary>Campo "enrol" da tabela "mdl_enrol" do Moodle</summary>  
        public virtual string TipoInscricao { get; set; }
    }
}
