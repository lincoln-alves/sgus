namespace Sebrae.Academico.Dominio.Classes.ConheciGame
{
    public class Partida
    {
        public virtual int ID { get; set; }
        public virtual UsuarioConheciGame Usuario { get; set; }
        public virtual Tema Tema { get; set; }
    }
}
