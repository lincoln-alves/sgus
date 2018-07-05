using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    /// <summary>
    /// Enumeração referente aos status do encaminhamento de uma etapa
    /// </summary>
    public enum enumStatusEncaminhamentoEtapa
    {
        [Description("Aguardando resposta")]
        Aguardando = 0,
        [Description("Recusado")]
        Negado = 1,
        [Description("Aceito")]
        Aprovado = 2
    }
}
