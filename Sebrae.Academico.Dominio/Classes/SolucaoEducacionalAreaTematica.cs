
namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalAreaTematica: EntidadeBasicaPorId
    {
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual AreaTematica AreaTematica { get; set; }

        public override bool Equals(object obj)
        {
            var objeto = obj as SolucaoEducacionalAreaTematica;
            return objeto == null ? false : SolucaoEducacional.Equals(objeto.SolucaoEducacional)
                && AreaTematica.Equals(objeto.AreaTematica);
        }

        public override int GetHashCode()
        {
            return SolucaoEducacional.ID.GetHashCode() + AreaTematica.ID.GetHashCode();
        }
    }
}
