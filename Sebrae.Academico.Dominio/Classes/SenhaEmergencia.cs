using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class SenhaEmergencia : EntidadeBasica
    {
        public virtual string Senha { get; set; }
        public virtual DateTime DataValidade { get; set; }
        public virtual Uf UF { get; set; }
    }
}
