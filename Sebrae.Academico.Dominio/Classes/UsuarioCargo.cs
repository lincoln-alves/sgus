namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioCargo
    {
        public virtual int ID { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Cargo Cargo { get; set; }
    }
}