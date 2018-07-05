
using System.ComponentModel;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusTurma
    {
        [Description("Prevista")]
        Prevista = 1,

        [Description("Confirmada")]
        Confirmada = 2,

        [Description("Cancelada")]
        Cancelada = 3,

        [Description("Em Andamento")]
        EmAndamento = 4,

        [Description("Realizada")]
        Realizada = 5
    }
}
