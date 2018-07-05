using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações das Permissões de cada solução Educacional, 
    /// que um usuário possui.
    /// </summary>
    public class DTOSolucaoEducacionalPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual int IdUsuario { get; set; }
    }
}
