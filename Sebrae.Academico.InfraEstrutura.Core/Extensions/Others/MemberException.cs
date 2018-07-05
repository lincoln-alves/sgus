using System;

namespace Sebrae.Academico.InfraEstrutura.Core.Extensions.Others
{
    public class MemberException : ArgumentException
    {
        public MemberException()
            : base("O Argumento não é uma propriedade, não um campo")
        {

        }
    }
}
