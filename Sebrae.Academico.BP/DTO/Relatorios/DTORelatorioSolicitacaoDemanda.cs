using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioSolicitacaoDemanda
    {
        public int NumeroProcesso { get; set; }
        public string UsuarioDemandante { get; set; }
        public string Unidade { get; set; }
        public string Demanda { get; set; }
        public DateTime DataAbertura { get; set; }
        public enumStatusProcessoResposta Status { get; set; }
        public string EtapaAtual { get; set; }
        public EtapaResposta EtapaResposta { get; set; }
        public int IdEtapaResposta { get; set; }
        public int IdUsuario { get; internal set; }
        public int IdProcesso { get; internal set; }
        public int IdEtapa { get; internal set; }
    }
}
