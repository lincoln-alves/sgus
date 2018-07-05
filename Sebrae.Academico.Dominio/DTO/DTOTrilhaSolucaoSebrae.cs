using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DtoTrilhaSolucaoSebrae
    {
        public int Id { get; set; }
        public int FormaAquisicaoId { get; set; }
        public string FormaAquisicao { get; set; }
        public string FormaAquisicaoImagem { get; set; }        
        public int PontoSebraeId { get; set; }
        public string Nome { get; set; }
        public string Orientacao { get; set; }
        public int? Moedas { get; set; }
        public bool DonoTrilha { get; set; }
        public enumTipoItemTrilha? Tipo { get; set; }
        public enumStatusParticipacaoItemTrilha? Status { get; set; }
        public enumOrigemItemTrilha Origem { get; set; }
        public int? MatriculaOfertaId { get; set; }
        public string LinkAcesso { get; set; }
        public int ID_TemaConheciGame { get; set; }
        public int MediaAvaliacoes { get; set; }
        public int TotalAvaliacoes { get; set; }
        public bool UsuarioAvaliou { get; set; }
        public int CargaHoraria { get; set; }
        public int MissaoID{ get; set; }
    }
}
