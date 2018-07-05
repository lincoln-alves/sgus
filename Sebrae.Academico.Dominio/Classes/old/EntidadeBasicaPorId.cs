using System;

namespace Sebrae.Academico.Trilhas.Dominio.Classes
{
    public abstract class EntidadeBasicaPorId 
    {
        public virtual Int32 ID { get; set; }

        public virtual DateTime? DtAlteracao { get; set; }

        public virtual string Usuario { get; set; }
        
    }
}
