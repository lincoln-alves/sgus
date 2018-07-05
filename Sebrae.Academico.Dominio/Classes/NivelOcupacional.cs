using System.Collections.Generic;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class NivelOcupacional: ICloneable
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }

        public virtual IList<EtapaPermissao> ListaEtapaPermissao { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
