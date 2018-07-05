using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOJsonResultDrupalCSRF
    {
        public string token { get; set; }

        public string error_msg { get; set; }
    }
}
