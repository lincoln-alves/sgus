using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogGeracaoRelatorio  
    {

        //public virtual int IDUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        //public virtual int IDRelatorio { get; set; }
        public virtual Relatorio Relatorio { get; set; }
        public virtual DateTime DTGeracao { get; set; }
        
        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            LogGeracaoRelatorio objeto = obj as LogGeracaoRelatorio;
            return objeto == null ? false : Usuario.Equals(objeto.Usuario)
                && Relatorio.Equals(Relatorio)
                && DTGeracao.Equals(DTGeracao);
        }

        public override int GetHashCode()
        {
            return Usuario.ID.GetHashCode() + Relatorio.ID.GetHashCode()
                + DTGeracao.GetHashCode();
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj == null) return false;
        //    var t = obj as LogGeracaoRelatorio;
        //    if (t == null) return false;
        //    if (IDUsuario == t.IDUsuario
        //     && IDRelatorio == t.IDRelatorio
        //     && DTGeracao == t.DTGeracao)
        //        return true;

        //    return false;
        //}

        //public override int GetHashCode()
        //{
        //    int hash = 13;
        //    hash += IDUsuario.GetHashCode();
        //    hash += IDRelatorio.GetHashCode();
        //    hash += DTGeracao.GetHashCode();

        //    return hash;
        //}
        
        #endregion

      
    }
}
