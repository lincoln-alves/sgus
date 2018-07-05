using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sebrae.Academico.WebForms.Cadastros.Oferta
{
    public class JSONResultReturn
    {
        public bool Sucesso { get; set; }

        public string Mensagem { get; set; }

        public int? IdGrupo { get; set; }
    }
}