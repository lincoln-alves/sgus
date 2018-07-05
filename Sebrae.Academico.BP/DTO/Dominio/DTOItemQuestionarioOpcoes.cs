namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOItemQuestionarioOpcoes
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public DTOItemQuestionario ItemQuestionario { get; set; }
        public bool RespostaCorreta { get; set; }
        public int TipoDiagnostico { get; set; }
        public byte? OpcaoInt { get; set; }
        public DTOItemQuestionarioOpcoes OpcaoVinculada { get; set; }
    }
}