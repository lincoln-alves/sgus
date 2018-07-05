using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações das Permissões de cada Programa, 
    /// que um usuário possui.
    /// </summary>
    public class DTOProgramaPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual Programa Programa { get; set; }
        public virtual int IdUsuario { get; set; }
    }
}
