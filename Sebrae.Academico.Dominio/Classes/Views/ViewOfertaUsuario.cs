using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes.Views
{
    public class ViewOfertaUsuario
    {
        public virtual int NumeroLinha { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
