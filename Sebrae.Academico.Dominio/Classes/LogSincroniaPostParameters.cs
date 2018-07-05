using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes{
    public class LogSincroniaPostParameters : EntidadeBasicaPorId{
        public virtual LogSincronia LogSincronia { get; set; }
        public virtual string Registro { get; set; }
        public virtual string Descricao { get; set; }

        #region ICloneable Members
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
