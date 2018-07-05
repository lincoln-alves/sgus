using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class LogAcessoFornecedor 
    {
        public virtual int IDFornecedor { get; set; }
        public virtual string Metodo { get; set; }
        public virtual DateTime DataAcesso { get; set; }

        public override bool Equals(object obj)
        {
            LogAcessoFornecedor objeto = obj as LogAcessoFornecedor;
            return objeto == null ? false : IDFornecedor.Equals(objeto.IDFornecedor)
                && DataAcesso.Equals(objeto.DataAcesso);
        }

        public override int GetHashCode()
        {
            return IDFornecedor.GetHashCode() + DataAcesso.GetHashCode();
        }
    }
}
