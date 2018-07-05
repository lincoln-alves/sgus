namespace Sebrae.Academico.Dominio.Classes{
    public class AreaTematicaPermissao : EntidadeBasica{
        public virtual AreaTematica AreaTematica { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
