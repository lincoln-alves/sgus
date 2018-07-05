using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusDemanda
    {
        [Description("Em Andamento")]
        EmAndamento = 0,
        [Description("Pendente")]
        Pendente = 1,
        [Description("Finalizada")]
        Finalizada = 2,
        [Description("Analisada")]
        Analisada = 3,
    }
}
