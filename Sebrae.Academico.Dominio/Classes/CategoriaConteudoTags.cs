namespace Sebrae.Academico.Dominio.Classes
{
    public class CategoriaConteudoTags : EntidadeBasicaPorId
    {
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
        public virtual Tag Tag { get; set; }

        public override bool Equals(object obj)
        {
            CategoriaConteudoTags objeto = obj as CategoriaConteudoTags;
            return objeto == null ? false : CategoriaConteudo.Equals(objeto.CategoriaConteudo)
                && Tag.Equals(objeto.Tag);
        }

        public override int GetHashCode()
        {
            return CategoriaConteudo.ID.GetHashCode() + Tag.ID.GetHashCode();
        }
    }
}