namespace Sebrae.Academico.Dominio.Classes
{
    public class EnglishTownUsuario : EntidadeBasica
    {
        public virtual string Sid { get; set; }
        public virtual string Mid { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
