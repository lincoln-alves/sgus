namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOUsuarioMapa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string Unidade { get; set; }
        public string FileServer { get; set; }
        public int MoedasOuro { get; set; }
        public int MoedasPrata { get; set; }
    }
}