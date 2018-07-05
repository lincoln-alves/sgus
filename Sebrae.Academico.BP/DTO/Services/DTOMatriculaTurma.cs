using Sebrae.Academico.BP.DTO.Dominio;
using System;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOMatriculaTurma 
    {
        public virtual string Cpf { get; set; }
        public virtual string StatusMatricula { get; set; }
        public virtual DateTime DataMatricula { get; set; }
        public virtual DateTime DataLimite { get; set; }
        public virtual double? Nota1 { get; set; }
        public virtual double? Nota2 { get; set; }
        public virtual double? ValorNotaProvaOnline { get; set; }
        public virtual double? MediaFinal { get; set; }
    }
}
