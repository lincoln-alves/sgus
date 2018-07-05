
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// Enumeração referente aos status das respostas de uma etapa
    /// </summary>
    public enum enumStatusEtapaResposta
    {
        [Description("Aguardando")]
        Aguardando = 0,
        [Description("Negado")]
        Negado = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Concluido")]
        Concluido = 3,
        [Description("A Analisar")]
        AAnalisar = 4,
        [Description("Analisado")]
        Analisado = 5,
        [Description("A Ajustar")]
        AAjustar = 6,
        [Description("Cancelado")]
        Cancelado = 99
    }
}
