using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioTrilhaMoedas : EntidadeBasicaPorId, ICloneable
    {
        public virtual ItemTrilha ItemTrilha { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual ItemTrilhaCurtida Curtida { get; set; }

        public virtual int MoedasDePrata { get; set; }
        public virtual int MoedasDeOuro { get; set; }

        public virtual enumTipoCurtida TipoCurtida { get; set; }
        
        public virtual DateTime DataCriacao { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
        
    }
}