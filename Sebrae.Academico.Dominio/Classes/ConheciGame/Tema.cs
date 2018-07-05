namespace Sebrae.Academico.Dominio.Classes.ConheciGame
{
    public class Tema
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual Conteudo Conteudo { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
