using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DtoSolucoesEConcluintesPorCategoria
    {
        public string Categoria { get; set; }
        public int QtdSolucoes { get; set; }
        public int Concluintes { get; set; }
    }

}
