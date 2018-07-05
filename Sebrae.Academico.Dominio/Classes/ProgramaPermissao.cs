
namespace Sebrae.Academico.Dominio.Classes
{
    public class ProgramaPermissao : EntidadeBasicaPorId
    {
        public virtual Programa Programa { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual Perfil Perfil { get; set; }
                
        public override int GetHashCode()
        {
            return Programa.ID.GetHashCode() + NivelOcupacional.ID.GetHashCode() + Uf.ID.GetHashCode()
                + Perfil.ID.GetHashCode();
        }
               
       
    }
}
