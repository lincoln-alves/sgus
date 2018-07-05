namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOMochila
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Unidade { get; set; }
        public string Foto { get; set; }
        public int MoedasOuro { get; set; }
        public int MoedasPrata { get; set; }
        public int Medalhas { get; set; }
        public int Ranking { get; set; }
        public int TotalMoedasNivel { get; set; }
        public bool TutorialLido { get; set; }

    }
}
