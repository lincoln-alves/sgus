using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcessoFuncionalidade 
    {
        public virtual int IDUsuario { get; set; }
        public virtual int IDFuncionalidade { get; set; }
        public virtual DateTime DataAcesso { get; set; }

        public override bool Equals(object obj)
        {
            LogAcessoFuncionalidade objeto = obj as LogAcessoFuncionalidade;
            return objeto == null ? false : IDUsuario.Equals(objeto.IDUsuario)
                && DataAcesso.Equals(objeto.DataAcesso);
        }

        public override int GetHashCode()
        {
            return IDUsuario.GetHashCode() + DataAcesso.GetHashCode();
        }
    }
}
