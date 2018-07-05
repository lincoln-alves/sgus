
namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalTags: EntidadeBasicaPorId
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual Tag Tag { get; set; }

        public override bool Equals(object obj)
        {
            SolucaoEducacionalTags objeto = obj as SolucaoEducacionalTags;
            return objeto == null ? false : SolucaoEducacional.Equals(objeto.SolucaoEducacional)
                && Tag.Equals(objeto.Tag);
        }

        public override int GetHashCode()
        {
            return SolucaoEducacional.ID.GetHashCode() + Tag.ID.GetHashCode();
        }
    }
}
