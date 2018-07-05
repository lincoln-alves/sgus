using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaTag : EntidadeBasica, ICloneable
    {

        public virtual Trilha Trilha { get; set; }
        public virtual Tag Tag { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            TrilhaTag objeto = obj as TrilhaTag;
            return objeto == null ? false : Trilha.Equals(objeto.Trilha)
                && Tag.Equals(objeto.Tag);
        }

        public override int GetHashCode()
        {
            return Trilha.ID.GetHashCode() + Tag.ID.GetHashCode();
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

}