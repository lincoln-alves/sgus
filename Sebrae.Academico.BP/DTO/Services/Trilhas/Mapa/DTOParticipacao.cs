namespace Sebrae.Academico.BP.DTO.Services.Trilhas.Mapa
{
    public class DTOParticipacao
    {
        public int IdItemTrilha { get; set; }
        public int IdTrilhaNivel { get; set; }
        public string TxParticipacao { get; set; }
        public string NomeDoArquivoOriginal { get; set; }
        public string Base64 { get; set; }
    }
}