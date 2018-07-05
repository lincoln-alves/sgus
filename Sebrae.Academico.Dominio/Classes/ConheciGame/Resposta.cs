namespace Sebrae.Academico.Dominio.Classes.ConheciGame
{
    public class Resposta
    {
        public virtual int ID { get; set; }
        public virtual UsuarioConheciGame Usuario { get; set; }
        public virtual Partida Partida { get; set; }
        public virtual bool Acertou { get; set; }   
    }
}
