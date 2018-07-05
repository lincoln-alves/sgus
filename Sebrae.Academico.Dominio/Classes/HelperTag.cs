namespace Sebrae.Academico.Dominio.Classes
{
    public class HelperTag
    {
        public virtual int ID { get; set; }
        public virtual string Chave { get; set; }
        public virtual string Descricao { get; set; }

        public virtual Pagina Pagina { get; set; }
    }
}
