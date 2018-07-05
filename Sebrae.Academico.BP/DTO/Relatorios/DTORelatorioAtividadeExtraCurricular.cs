using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioAtividadeExtraCurricular
    {
        public string NomeInstituicao { get; set; }
        public string SolucaoEducacional { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public int CargaHoraria { get; set; }
        public string DescricaoSolucao { get; set; }
        public int? idFileServer { get; set; }
        public DateTime? DataAtualizacao { get; set; }  

        public string linkToFile { get; set; }
    }
}
