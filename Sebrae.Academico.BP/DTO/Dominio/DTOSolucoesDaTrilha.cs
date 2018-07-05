using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOSolucoesDaTrilha
    {
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }
        public int Moedas { get; set; }                
           
    }

    public class DTOCursosOnlineUCSebrae
    {
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }
        public int Moedas { get; set; }                        
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

    }

    public class DTOSolucoesTrilheiro
    {
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }        
        public int MoedasOuro { get; set; }
        public int MoedasPratas { get; set; }                
    }

    public class DTOSolucaoesDesempenhoGeral
    {        
        public int Medalhas { get; set; }
        public int HorasCertificadas { get; set; }
        public int Solucoes { get; set; }
        public int Moedas { get; set; }
        public int Trofeus { get; set; }     
        public int HorasRegistradas { get; set; }        

    }
}
