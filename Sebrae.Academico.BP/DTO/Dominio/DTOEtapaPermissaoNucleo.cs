using Sebrae.Academico.BP.DTO.Services.Processo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOEtapaPermissaoNucleo
    {
        public int ID_EtapaPermissaoNucleo { get; set; }
        public int ID_Etapa { get; set; }
        public int ID_Usuario { get; set; }
        public int ID_Nucleo { get; set; }
        public string NomeUsuario { get; set; }
        public string Nucleo { get; set; }
        public bool? Gestor { get; set; }
    }
}
