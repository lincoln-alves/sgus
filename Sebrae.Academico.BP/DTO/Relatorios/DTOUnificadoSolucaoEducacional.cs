using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOUnificadoSolucaoEducacional
    {
        public int ID { get; set; }
        public string SE { get; set; }
        public string FormaAquisicao { get; set; }
        public string Categoria { get; set; }
        public string Fornecedor { get; set; }
        public string CargaHoraria { get; set; }
        public string Ativo { get; set; }
        public int QntOfertas { get; set; }
        public int QntTurmas { get; set; }
        public string UFResponsavel { get; set; }
    }
}
