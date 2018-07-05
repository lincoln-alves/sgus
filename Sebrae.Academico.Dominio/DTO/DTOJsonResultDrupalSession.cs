using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOJsonResultDrupalSession
    {
        public string sessid { get; set; }
        public string session_name { get; set; }
        public string token { get; set; }

        public string error_msg { get; set; }
    }
}
