
namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações dos Logs de Funcionalidades
    /// </summary>
    public class DTOFuncionalidade
    {
        public virtual string NomeFuncionalidade { get; set; }
        public virtual string LinkFuncionalidade { get; set; }
        public virtual int Qtd { get; set; }
    }
}
