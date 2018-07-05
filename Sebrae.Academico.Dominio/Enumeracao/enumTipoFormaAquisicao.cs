using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumTipoFormaAquisicao
    {
        [Description("Solução Educacional")]
        SolucaoEducacional = 1,
        [Description("Trilha")]
        Trilha = 2
    }
}
