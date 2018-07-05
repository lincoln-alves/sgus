namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOSolucaoEducacionalFormaAquisicao
    {
        public string FormaAquisicao { get; set; }
        public string Solucoes { get; set; }
        public int? ID_SolucaoEducacional { get; set; }

        public string SolucoesFormatado
        {
            get { return Solucoes ?? "N/D"; }
        }

        public string UFResponsavel { get; set; }
    }
}
