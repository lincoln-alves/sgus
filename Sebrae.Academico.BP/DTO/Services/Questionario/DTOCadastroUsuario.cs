
namespace Sebrae.Academico.BP.DTO.Services.Questionario
{
    public class DTOCadastroQuestionarioParticipacao
    {
        public int IdUsuario { get; set; }
        public int IdTrilhaNivel { get; set; }
        public int IdTurma { get; set; }
        public int IdTurmaCapacitacao { get; set; }
        public int? IdItemTrilha { get; set; }
        public bool Pre { get; set; }
        public bool Pos { get; set; }
        public bool Abandono { get; set; }
        public bool Cancelamento { get; set; }
        public bool Prova { get; set; }
        public bool Evolutivo { get; set; }
        public bool AtividadeTriha { get; set; }
        public bool Eficacia { get; set; }
    }
}
