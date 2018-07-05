using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DtoSolucoesEducacionaisPorCategoria
    {
        public string Categoria { get; set; }
        public string SolucaoEducacional { get; set; }
        public int Concluintes { get; set; }
    }
}
