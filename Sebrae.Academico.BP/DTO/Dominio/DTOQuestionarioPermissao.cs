using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOQuestionarioPermissao
    {
        public DTONivelOcupacional NivelOcupacional { get; set; }
        public DTOUf Uf { get; set; }
        public DTOPerfil Perfil { get; set; }
        public DTOFormaAquisicao FormaAquisicao { get; set; }
    }
}
