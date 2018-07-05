using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOSolucoesPorCategoria
    {
        public string SolucaoEducacional { get; set; }
        public string Categoria { get; set; }
        public string SubCategoria { get; set; }
        public string Fornecedor { get; set; }
        public string FormaAquisicao { get; set; }
        public int? CargaHoraria { get; set; }
        public string Ativo { get; set; }
        public string UFResponsavel { get; set; }
    }
}
