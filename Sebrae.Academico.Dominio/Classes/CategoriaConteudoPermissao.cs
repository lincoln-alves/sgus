using System;
namespace Sebrae.Academico.Dominio.Classes
{
    public class CategoriaConteudoPermissao : EntidadeBasica
    {
        public virtual CategoriaConteudo CategoriaConteudo { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
