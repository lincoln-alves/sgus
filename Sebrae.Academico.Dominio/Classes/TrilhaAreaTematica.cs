
namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaAreaTematica: EntidadeBasicaPorId
    {
        public virtual Trilha Trilha { get; set; }
        public virtual AreaTematica AreaTematica { get; set; }

        public override bool Equals(object obj)
        {
            var objeto = obj as TrilhaAreaTematica;
            return objeto == null ? false : Trilha.Equals(objeto.Trilha)
                && AreaTematica.Equals(objeto.AreaTematica);
        }

        public override int GetHashCode()
        {
            return Trilha.ID.GetHashCode() + AreaTematica.ID.GetHashCode();
        }
    }
}
