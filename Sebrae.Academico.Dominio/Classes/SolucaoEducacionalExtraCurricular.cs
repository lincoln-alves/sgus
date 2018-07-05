using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolucaoEducacionalExtraCurricular : EntidadeBasica
    {
        public virtual FormaAquisicao FormaAquisicao { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
