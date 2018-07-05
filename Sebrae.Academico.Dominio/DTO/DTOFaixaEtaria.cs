using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOFaixaEtaria
    {
        public virtual string Nome { get; set; }
        public virtual int Idade { get; set; }
        public virtual float Quantidade { get; set; }
    }
}
