using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcesso 
    {
        public virtual int IDUsuario { get; set; }
        public virtual DateTime DataAcesso { get; set; }
        public virtual bool? INSGUS { get; set; }
        public virtual string IP { get; set; }
        public virtual string SessionID { get; set; }

        public LogAcesso(int idUsuario, bool? inSgus, string IP, string sessionID)
        {
            this.IDUsuario = idUsuario;
            this.INSGUS = inSgus;
            this.DataAcesso = DateTime.Now;
            this.IP = IP;
            this.SessionID = sessionID;
        }

        public LogAcesso()
        {

        }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as LogAcesso;
            if (t == null) return false;
            if (IDUsuario == t.IDUsuario
             && DataAcesso == t.DataAcesso)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += IDUsuario.GetHashCode();
            hash += DataAcesso.GetHashCode();

            return hash;
        }
        #endregion
    }
}

