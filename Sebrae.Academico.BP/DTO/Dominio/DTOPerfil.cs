namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOPerfil : DTOEntidadeBasica
    {
        public DTOPerfil()
        {
            IsHabilitado = true;
        }

        public bool IsHabilitado { get; set; }
        public bool IsSelecionado { get; set; }
    }
}
