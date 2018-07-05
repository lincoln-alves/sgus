using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioSEOfertaMatricula
    {
        public string SolucaoEducacional { get; set; }
        public string SolucaoEducacional2 { get; set; }
        public string Oferta { get; set; }
        public string TipoOferta { get; set; }
        public DateTime? DataInicioInscricoes { get; set; }
        public DateTime? DataFimInscricoes { get; set; }
        public int QtdMaxInscricoes { get; set; }
        public int Inscritos { get; set; }
        public int FilaEspera { get; set; }
        public int Solicitado { get; set; }
        public string UFResponsavel { get; set; }
        public int ID_Oferta { get; set; }
    }
}
