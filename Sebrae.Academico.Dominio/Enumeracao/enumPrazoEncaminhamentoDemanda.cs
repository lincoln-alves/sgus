using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumPrazoEncaminhamentoDemanda
    {
        [Description("No Prazo")]
        NoPrazo,

        [Description("A Expirar")]
        AExpirar,

        [Description("Fora do Prazo")]
        ForaDoPrazo,

        [Description("Encerrada")]
        Encerrada
    }
}
