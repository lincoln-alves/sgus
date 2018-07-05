using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Tag : EntidadeBasica, ICloneable
    {
        public virtual Tag TagPai { get; set; }
        public virtual bool? InSinonimo { get; set; }
        public virtual IList<Tag> ListaTagFilhos { get; set; }
        public virtual byte? NumeroNivel { get; set; }
        public Tag()
        {
            ListaTagFilhos = new List<Tag>();
        }


        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
