
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumDominio
    {
        [Description("DD - Demonstra Domínio")]
        Domina = 1,
        [Description("DP - Demonstra Parcial Domínio")]
        DominioParcial = 2,
        [Description("ND - Não Demonstra Domínio")]
        NaoDominia = 3,
        [Description("NA/NO – Não se aplica ou não observado")]
        NaoSeAplica = 4
    }
}
