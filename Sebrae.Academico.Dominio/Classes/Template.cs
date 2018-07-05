using System.Collections.Generic;
using System;
namespace Sebrae.Academico.Dominio.Classes
{
    public class Template : EntidadeBasicaPorId
    {
        public virtual string TextoTemplate { get; set; }
        public virtual string DescricaoTemplate { get; set; }
        public virtual string HashTag { get; set; }
        public virtual string Assunto { get; set; }
    }
}
