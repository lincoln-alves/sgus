using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// Enumeração Referente aos Tipos de Questionário usados em uma Associação de Questionário.
    /// Indica o comportamento do questionário, em que momento ele será apresentado. ex: Pré, pós.
    /// </summary>
    public enum enumTipoQuestionarioAssociacao
    {
        [Description("Pré")]
        Pre = 1,
        [Description("Pós")]
        Pos = 2,
        Prova = 3,
        Avulso = 4,
        Cancelamento = 5,
        Abandono = 6,
        Demanda = 7,
        AtividadeTrilha = 8,
        Eficacia = 9
    }
}