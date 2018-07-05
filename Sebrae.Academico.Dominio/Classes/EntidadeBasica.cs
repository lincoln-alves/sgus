using System;
using Sebrae.Academico.Dominio.Attributes.TestValidationAttributes;

namespace Sebrae.Academico.Dominio.Classes
{
    /// <summary>
    /// Classe Básica.
    /// </summary>
    public abstract class EntidadeBasica : EntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
    }
}
