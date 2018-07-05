using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ProgramaTag : EntidadeBasica
    {
        public virtual Programa Programa { get; set; }
        public virtual Tag Tag { get; set; }

        public override bool Equals(object obj)
        {
            ProgramaTag objeto = obj as ProgramaTag;
            return objeto == null ? false : Programa.Equals(objeto.Programa)
                && Tag.Equals(objeto.Tag);
        }

        public override int GetHashCode()
        {
            return Programa.ID.GetHashCode() + Tag.ID.GetHashCode();
        }
    }

}