using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemTrilhaCurtida : EntidadeBasicaPorId, ICloneable
    {
        public virtual ItemTrilha ItemTrilha { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual enumTipoCurtida TipoCurtida { get; set; }
        public virtual int ValorCurtida { get; set; }
        public virtual int ValorDescurtida { get; set; }

        public virtual DateTime DataCriacao { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
        
    }
}