using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOConcluintesPorFormaAquisicao
    {
        public virtual string FormaAquisicao { get; set; }
        public virtual decimal Percentual { get; set; }
    }
}
