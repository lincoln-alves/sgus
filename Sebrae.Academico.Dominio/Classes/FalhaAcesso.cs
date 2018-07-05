using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class FalhaAcesso : EntidadeBasica
    {
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual string IP { get; set; }
        public virtual DateTime DataAcesso { get; set; }

        public FalhaAcesso(string login, String senha, String iP)
        {
            this.Login = login;
            this.Senha = senha;
            this.IP = iP;
            this.DataAcesso = DateTime.Now;
        }

        public FalhaAcesso()
        {
        }
    }

}