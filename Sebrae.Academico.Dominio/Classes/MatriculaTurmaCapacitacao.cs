using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class MatriculaTurmaCapacitacao : EntidadeBasicaPorId
    {
        public MatriculaTurmaCapacitacao()
        {
            TurmaCapacitacao = new TurmaCapacitacao();
        }

        public virtual TurmaCapacitacao TurmaCapacitacao { get; set; }
        public virtual DateTime DataMatricula { get; set; }
        public virtual DateTime? DataLimite { get; set; }
        public virtual MatriculaCapacitacao MatriculaCapacitacao { get; set; }
        public virtual double? ValorNotaProvaOnline { get; set; }
    }
}