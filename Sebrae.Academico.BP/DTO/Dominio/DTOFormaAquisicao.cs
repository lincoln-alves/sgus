
namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOFormaAquisicao : DTOEntidadeBasica
    {
        public string Imagem { get; set; }
        public string CargaHoraria { get; set; }
        public bool PermiteAlterarCargaHoraria { get; set; }
        
        public int Quantidade { get; set; }
    }

} 
