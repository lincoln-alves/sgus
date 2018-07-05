
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumSituacaoUsuarioCertificadoCertame
    {
        [Description("Ausente")]
        Ausente = 0,
        [Description("Presente")]
        Presente = 1
    }
    public enum enumStatusUsuarioCertificadoCertame
    {
        [Description("Reprovado")]
        Reprovado = 0,
        [Description("Aprovado")]
        Aprovado = 1
    }
}