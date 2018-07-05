using System;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// Enumeração Referente aos Tipos de Questionário.
    /// </summary>
    [Serializable]
    public enum enumTipoQuestionario
    {
        AvaliacaoProva = 1,
        Pesquisa = 2,
        Dinamico = 3,
        Avulso = 4,
        Cancelamento = 5,
        Abandono = 6,
        Demanda = 7,
        AtividadeTrilha = 8
    }
}