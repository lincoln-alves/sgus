namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTOOrcamentoReembolso
    {
        public int ID { get; set; }
        public int Ano { get; set; }
        public decimal Total { get; set; }
        public decimal Utilizado { get; set; }
        public decimal Saldo => Total - Utilizado;
    }
}
