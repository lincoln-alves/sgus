using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumStatusProcessoResposta
    {
        [Description("Cancelado")]
        Cancelado,
        [Description("Ativo")]
        Ativo
    }
}
