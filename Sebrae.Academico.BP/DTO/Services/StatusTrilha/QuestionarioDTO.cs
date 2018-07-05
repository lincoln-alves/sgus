

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class QuestionarioDTO
    {
        public virtual string Tipo { get; set; }
        public virtual bool Existe { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual bool Preenchido { get; set; }
    }
}
