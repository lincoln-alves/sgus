using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumTipoProcesso
    {
        [Description("Reembolso")]
        Reembolso = 1,
        [Description("Outros")]
        Outros = 2
    }
}
