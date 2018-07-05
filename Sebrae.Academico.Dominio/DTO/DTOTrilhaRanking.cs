namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOTrilhaRanking
    {
        public virtual int ID { get; set; }
        public virtual long Ordem { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Unidade { get; set; }
        public virtual int? Medalhas { get; set; }
        public virtual int? MoedasOuro { get; set; }
        public virtual int? MoedasPrata { get; set; }
        public virtual int? Trofeu { get; set; }
        public virtual string Email { get; set; }
    }
}
