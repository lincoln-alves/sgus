
namespace Sebrae.Academico.Dominio.Classes
{
    public class ProgramaAreaTematica: EntidadeBasicaPorId
    {
        public virtual Programa Programa { get; set; }
        public virtual AreaTematica AreaTematica { get; set; }

        public override bool Equals(object obj)
        {
            var objeto = obj as ProgramaAreaTematica;
            return objeto == null ? false : Programa.Equals(objeto.Programa)
                && AreaTematica.Equals(objeto.AreaTematica);
        }

        public override int GetHashCode()
        {
            return Programa.ID.GetHashCode() + AreaTematica.ID.GetHashCode();
        }
    }
}
