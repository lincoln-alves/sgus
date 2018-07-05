namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioPrograma
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int QtdSolucoes { get; set; }
        public int QtdInscritos { get; set; }
        public int QtdAprovados { get; set; }
        public string Cpf { get; set; }
        public string Usuario { get; set; }
        public string NivelOcupacional { get; set; }
        public string UF { get; set; }
        public string StatusMatricula { get; set; }
    }
}
