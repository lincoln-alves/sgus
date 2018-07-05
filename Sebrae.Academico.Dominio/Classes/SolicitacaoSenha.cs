using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SolicitacaoSenha: EntidadeBasicaPorId
    {
        public virtual Usuario Usuario { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime DataValidade { get; set; }
    }
}
