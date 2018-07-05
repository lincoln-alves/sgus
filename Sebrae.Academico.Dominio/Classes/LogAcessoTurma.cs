using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcessoTurma
    {
        public virtual DateTime DataAcesso { get; set; }
        public virtual int IDMatriculaTurma { get; set; }

        public LogAcessoTurma()
        {

        }
        public LogAcessoTurma(int idMatriculaTurma)
        {
            this.IDMatriculaTurma = idMatriculaTurma;
            DataAcesso = DateTime.Now;
        }
        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as LogAcessoTurma;
            if (t == null) return false;
            if (IDMatriculaTurma == t.IDMatriculaTurma
             && DataAcesso == t.DataAcesso)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash += IDMatriculaTurma.GetHashCode();
            hash += DataAcesso.GetHashCode();

            return hash;
        }

        #endregion
    }

}

