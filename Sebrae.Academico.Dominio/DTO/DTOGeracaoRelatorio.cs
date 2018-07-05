
namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações dos Logs de Geração de Relatório
    /// </summary>
    public class DTOGeracaoRelatorio
    {
        public virtual int IdRelatorio { get; set; }
        public virtual string NomeRelatorio { get; set; }
        public virtual string LinkRelatorio { get; set; }
        public virtual int Qtd { get; set; }
    }
}
