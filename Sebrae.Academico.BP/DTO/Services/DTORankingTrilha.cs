using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTORankingTrilha
    {
        public byte Posicaoranking { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string CPFUsuario { get; set; }
        public string EstadoUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public string UFSigla { get; set; }
        public string UF { get; set; }
        public byte QuantidadeEstrelasPossiveis { get; set; }
        public byte QuantidadeEstrelas { get; set; }
        public enumStatusMatricula StatusMatricula { get; set; }
        public string Nivel { get; set; }
    }
}
