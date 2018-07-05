using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcessoTrilha
    {
        public virtual int IDUsuarioTrilha { get; set; }
        public virtual DateTime DataAcesso { get; set; }

        public LogAcessoTrilha()
        {

        }
        public LogAcessoTrilha(int idUsuarioTrilha)
        {
            this.IDUsuarioTrilha = idUsuarioTrilha;
            DataAcesso = DateTime.Now;
        }
        
        #region NHibernate Composite Key Requirements
      
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as LogAcessoTrilha;
            if (t == null) return false;
            if (IDUsuarioTrilha == t.IDUsuarioTrilha
             && DataAcesso == t.DataAcesso)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += IDUsuarioTrilha.GetHashCode();
            hash += DataAcesso.GetHashCode();

            return hash;
        }

        #endregion
    }
}


