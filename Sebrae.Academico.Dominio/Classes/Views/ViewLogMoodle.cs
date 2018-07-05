using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Views
{
    public class ViewLogMoodle
    {
        public virtual int ID { get; set; }
        public virtual int ID_Usuario { get; set; }
        public virtual string Usuario { get; set; }
        //public virtual string NomeUsuario { get; set; }
        //public virtual int ID_Curso { get; set; }
        //public virtual string Action { get; set; }
        //public virtual string Module { get; set; }
        //public virtual string NomeOferta { get; set; }
        public virtual int ID_Oferta { get; set; }
    }
}
