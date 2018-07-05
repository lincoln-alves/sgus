
namespace Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional
{
    public class DTOSolucEducSolucaoEducacional
    {
        public int CodigoSolucaoEducacional { get; set; }
        public string Nome { get; set; }
        public DTOSolucEducSolucaoEducacionalMatricula SolucaoEducacionalMatricula { get; set; }
    }
}
