using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações das Permissões de cada nivel de uma trilha
    /// que um usuário possui.
    /// </summary>
    public class DTOTrilhaNivelPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual int IdUsuario { get; set; }
    }
}