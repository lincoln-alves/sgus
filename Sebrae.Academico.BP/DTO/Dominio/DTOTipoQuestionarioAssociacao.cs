
namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOTipoQuestionarioAssociacao: DTOEntidadeBasica
    {
        public virtual bool Obrigatorio { get; set; }
        public virtual bool Evolutivo { get; set; }
    }
}
