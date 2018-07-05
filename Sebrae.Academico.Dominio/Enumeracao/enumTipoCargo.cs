using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// Tipos dos cargos da hierarquia. Ordenado do maior para o menor.
    /// </summary>
    public enum EnumTipoCargo
    {
        /// <summary>
        /// Todas as 3 diretorias principais:
        ///     - Diretoria de Administração e Finanças;
        ///     - Presidência (é visto como uma diretoria);
        ///     - Diretoria Técnica.
        /// </summary>
        [Description("Diretor")]
        Diretoria,
        /// <summary>
        /// Gabinetes das diretorias.
        /// </summary>
        [Description("Chefe de Gabinete")]
        Gabinete,
        /// <summary>
        /// Gerentes das unidades.
        /// </summary>
        [Description("Gerente")]
        Gerencia,
        /// <summary>
        /// Gerentes Adjuntos das unidades.
        /// </summary>
        [Description("Gerente Adjunto")]
        GerenciaAdjunta,
        /// <summary>
        /// Funcionários das unidades.
        /// </summary>
        [Description("Funcionário")]
        Funcionario
    }
}