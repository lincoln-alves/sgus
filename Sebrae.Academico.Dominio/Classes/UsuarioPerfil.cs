using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioPerfil : EntidadeBasicaPorId
    {
        public virtual Usuario Usuario { get; set; }
        public virtual Perfil Perfil { get; set; }

        public override bool Equals(object obj)
        {
            UsuarioPerfil objeto = obj as UsuarioPerfil;
            return objeto == null ? false : Usuario.Equals(objeto.Usuario)
                && Perfil.Equals(objeto.Perfil);


            //if (obj == null)
            //    return false;
            //var t = obj as UsuarioPerfil;
            //if (t == null)
            //    return false;
            //if (Usuario == t.Usuario && Perfil == t.Perfil)
            //    return true;
            //return false;
        }

        public override int GetHashCode()
        {
            return Usuario.ID.GetHashCode() + Perfil.ID.GetHashCode();
           // return (Usuario.ID + "|" + Perfil.ID).GetHashCode(); 
        }
    }
}
