using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.Dominio.Enumeracao
{
    public enum enumTipoSolucao
    {
        [Description("Solução Sebrae")]
        SolucaoSebrae = 0,
        [Description("Solução Trilheiro")]
        SolucaoTrilheiro = 1
    }
}
