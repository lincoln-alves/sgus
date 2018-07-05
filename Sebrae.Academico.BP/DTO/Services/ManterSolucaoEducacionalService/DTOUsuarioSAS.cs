using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Services.ManterSolucaoEducacionalService
{
    public class DTOUsuarioSAS
    {
        public string Cpf { get; set; }
        public string SSID { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int Sexo { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
    }
}
