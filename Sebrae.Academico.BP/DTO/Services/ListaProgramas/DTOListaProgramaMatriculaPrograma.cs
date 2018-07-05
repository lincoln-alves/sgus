
namespace Sebrae.Academico.BP.DTO.Services.ListaProgramas
{
    public class DTOListaProgramaMatriculaPrograma
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string UF { get; set; }
        public virtual string NivelOcupacional { get; set; }
        public virtual string StatusMatricula { get; set; }
    }
}
