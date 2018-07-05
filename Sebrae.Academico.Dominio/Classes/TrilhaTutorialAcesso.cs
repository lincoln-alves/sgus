using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaTutorialAcesso : EntidadeBasica
    {
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual enumCategoriaTrilhaTutorial Categoria { get; set; }        
    }
}

