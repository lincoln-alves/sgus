using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações das Permissões de cada Sistema Externo, 
    /// que um usuário possui.
    /// </summary>
    public class DTOSistemaExternoPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual SistemaExterno SistemaExterno { get; set; }
        public virtual int IdUsuario { get; set; }
    }
}
