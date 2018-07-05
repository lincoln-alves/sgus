using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumTrilhaMapa
    {
        [Description("Parque")]
        Parque = 0,
        [Description("Velho oeste")]
        VelhoOeste = 1,
        [Description("Neve")]
        Neve = 2,
        [Description("Fazenda")]
        Fazenda = 3,
        [Description("Cidade")]
        Cidade = 4,
        [Description("Praia")]
        Praia = 5
    }
}
