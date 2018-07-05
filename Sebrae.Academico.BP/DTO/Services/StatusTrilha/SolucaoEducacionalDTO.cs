
namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class SolucaoEducacionalDTO
    {
        public virtual string Nome { get; set; }
        public virtual int NodeId { get; set; }
        public virtual int IdNodePortal { get; set; }
        public bool SolucaoObrigatoria { get; set; }
    }
}
