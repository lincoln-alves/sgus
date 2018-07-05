using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusAvaliacao
    {
        [Description("Aguardando resposta")]
        AguardandoResposta,

        [Description("Aguardando aprovação do Gestor")]
        AguardandoGestor,

        [Description("Aprovada pelo Gestor")]
        Aprovada
    }
}
