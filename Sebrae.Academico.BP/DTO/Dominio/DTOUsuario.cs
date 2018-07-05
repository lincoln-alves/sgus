
namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOUsuario : DTOEntidadeBasicaPorId
    {

        public virtual int ID_Usuario { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Cpf { get; set; }
        public virtual string Unidade { get; set; }
        public virtual string NomeNivel { get; set; }
        public virtual string NomeUF { get; set; }
        public virtual DTOUf UF { get; set; }
        public virtual DTONivelOcupacional NivelOcupacional { get; set; }
        public virtual string Email { get; set; }
        
    }
}
