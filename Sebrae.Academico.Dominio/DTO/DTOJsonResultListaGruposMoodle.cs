using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOJsonResultListaGruposMoodle
    {
        public bool Sucesso { get; set; }

        public DTOJsonListaGrupoMoodle Grupos { get; set; }
    }
}
