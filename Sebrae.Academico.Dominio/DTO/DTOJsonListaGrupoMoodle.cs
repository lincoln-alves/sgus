using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOJsonListaGrupoMoodle
    {
        public string model { get; set; }

        public List<DTOGrupoMoodle> collection { get; set; }
    }
}
