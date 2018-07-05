using System;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOItemTrilha
    {
        public virtual string Nome { get; set; }
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual string NotaObtida { get; set; }
        public virtual string Missao { get; set; }
        public virtual string AprovadoStatus { get; set; }
        public virtual string AtivoSimNao { get; set; }
        public virtual string TipoSolucao { get; set; }
        public virtual string PontoSebrae { get; set; }
        public virtual string FormaDeAquisicao { get; set; }
        public virtual int TotalCurtidas { get; set; }
        public virtual int TotalDescurtidas { get; set; }
        public virtual long PosicaoRanking { get; set; }

        public virtual string DataInicioFormatado
        {
            get { return DataInicio?.ToString("dd/MM/yyyy") ?? "--"; }
        }

        public virtual string DataFimFormatado
        {
            get { return DataFim?.ToString("dd/MM/yyyy") ?? "--"; }
        }
    }
}