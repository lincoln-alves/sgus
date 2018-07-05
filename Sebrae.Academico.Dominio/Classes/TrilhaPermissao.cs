using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaPermissao : EntidadeBasicaPorId, ICloneable
    {
        public virtual Trilha Trilha { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Uf Uf { get; set; }


        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
