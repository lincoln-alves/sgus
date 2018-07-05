using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOSolucaoEducacionalOferta
    {
        public string SolucaoEducacional { get; set; }
        public string Oferta { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TipoOferta { get; set; }
        public DateTime? DataInicioInscricoes { get; set; }
        public DateTime? DataFimInscricoes { get; set; }
        public int MaxInscricoes { get; set; }
        public int Inscritos { get; set; }
        public int FilaEspera { get; set; }
        public int Solicitado { get; set; }
        public string UFResponsavel { get; set; }
        public int ID_SolucaoEducacional { get; set; }
    }
}
