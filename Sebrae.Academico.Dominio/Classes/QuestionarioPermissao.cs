using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class QuestionarioPermissao : EntidadeBasica, ICloneable
    {
        public virtual Questionario Questionario { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual FormaAquisicao FormaAquisicao { get; set; }

        #region ICloneable Members

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
