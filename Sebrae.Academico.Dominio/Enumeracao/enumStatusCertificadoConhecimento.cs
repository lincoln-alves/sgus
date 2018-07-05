using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusCertificadoConhecimento
    {
        [Description("Reprovado")]
        Reprovado = 0,
        [Description("Aprovado")]
        Aprovado = 1,
        [Description("Ausente")]
        Ausente = 2
    }
}