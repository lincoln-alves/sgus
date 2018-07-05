namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOUsuarioPerfil : DTOEntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
        public virtual string CPF { get; set; }
        public virtual string Email { get; set; }
        public virtual string NivelOcupacional { get; set; }
    }
}
