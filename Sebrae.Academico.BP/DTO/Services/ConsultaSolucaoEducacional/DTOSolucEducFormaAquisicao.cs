using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional
{
    public class DTOSolucEducFormaAquisicao
    {
        public string Nome { get; set; }
        public int CodigoFormaAquisicao { get; set; }
        public List<DTOSolucEducSolucaoEducacional> ListaSolucaoEducacional { get; set; }
    }
}
