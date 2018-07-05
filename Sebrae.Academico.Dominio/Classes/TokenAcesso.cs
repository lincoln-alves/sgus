using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TokenAcesso : EntidadeBasicaPorId
    {
        public TokenAcesso()
        {
            Usuario = new Usuario();
            Fornecedor = new Fornecedor();
        }

        public virtual Usuario Usuario { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Guid Token { get; set; }
        public virtual DateTime DataCriacao { get; set; }
        public virtual string IpAcesso { get; set; }
        public virtual string TokenMD5 { get; set; }  
    }
}
