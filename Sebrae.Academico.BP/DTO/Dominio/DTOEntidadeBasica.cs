
namespace Sebrae.Academico.BP.DTO.Dominio
{
    /// <summary>
    /// Classe Básica.
    /// </summary>
    public abstract class DTOEntidadeBasica : DTOEntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
        public virtual string Link { get; set; }
        public virtual bool InscricoesAbertas { get; set; }
    }
}
