
using System;
namespace Sebrae.Academico.Dominio.Classes
{
    public class LogBuscaSite 
    {
        public virtual int IDUsuario { get; set; }
        public virtual DateTime DataEvento { get; set; }
        public virtual String Busca { get; set; }


        public LogBuscaSite(int idUsuario, String busca)
        {
            this.IDUsuario = idUsuario;
            this.Busca = busca;
            this.DataEvento = DateTime.Now;
        }

        public LogBuscaSite()
        {

        }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as LogAcesso;
            if (t == null) return false;
            if (IDUsuario == t.IDUsuario
             && DataEvento == t.DataAcesso)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += IDUsuario.GetHashCode();
            hash += DataEvento.GetHashCode();

            return hash;
        }
        #endregion
    }
}

