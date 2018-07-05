
namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOUf
    {
        public DTOUf()
        {
            IsHabilitado = true;
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public bool IsHabilitado { get; set; }
        public bool IsSelecionado { get; set; }
        public int? Vagas { get; set; }
    }
}
