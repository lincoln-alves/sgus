
namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTONivelOcupacional : DTOEntidadeBasica
    {
        public DTONivelOcupacional()
        {
            IsHabilitado = true;
        }

        public bool IsHabilitado { get; set; }
        public bool IsSelecionado { get; set; }
    }
}
