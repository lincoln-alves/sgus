
namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaNivelPermissao : EntidadeBasica
    {
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Uf Uf { get; set; }
    }
}
