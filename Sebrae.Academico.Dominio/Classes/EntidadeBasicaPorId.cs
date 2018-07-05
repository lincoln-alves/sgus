using System;
using Sebrae.Academico.Dominio.Attributes.TestValidationAttributes;

namespace Sebrae.Academico.Dominio.Classes
{
    public class EntidadeBasicaPorId
    {
        public virtual int ID { get; set; }
        
        public virtual DateTime? DataAlteracao
        {
            get
            {
                return this.Auditoria.DataAuditoria;
            }
            private set
            {
                this.Auditoria.DataAuditoria = value;
            }
        }
        
        public virtual Auditoria Auditoria { get; set; }
        
        public virtual string UsuarioAlteracao
        {
            get
            {
                return this.Auditoria.UsuarioAuditoria;
            }
            private set
            {
                this.Auditoria.UsuarioAuditoria = value;
            }
        }

        public EntidadeBasicaPorId()
        {
            this.Auditoria = new Auditoria();
        }
    }
}
