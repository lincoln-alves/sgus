using System;

namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    /// <summary>
    /// Classe Básica.
    /// </summary>
    public abstract class EntidadeBasica: EntidadeBasicaPorId
    {
        public virtual string Nome { get; set; }
    }
}
