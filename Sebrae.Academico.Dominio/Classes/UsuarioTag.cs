using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioTag : EntidadeBasicaPorId
    {
        public virtual Tag Tag { get; set; }
        public virtual DateTime? DataValidade { get; set; }
        public virtual bool? Adicionado { get; set; }
        public virtual Usuario Usuario { get; set; }

        public override bool Equals(object obj)
        {
            UsuarioTag objeto = obj as UsuarioTag;
            return objeto == null ? false : Usuario.Equals(objeto.Usuario)
                && Tag.Equals(objeto.Tag);
        }

        public override int GetHashCode()
        {
            return Usuario.ID.GetHashCode() + Tag.ID.GetHashCode();
        }
    }
}
