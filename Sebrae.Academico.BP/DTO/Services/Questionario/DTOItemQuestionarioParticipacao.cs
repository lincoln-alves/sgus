using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services.Questionario
{
    public class DTOItemQuestionarioParticipacao : DTOEntidadeBasicaPorId
    {
        public virtual DTOTipoItemQuestionario TipoItemQuestionario { get; set; }
        public virtual DTOEstiloItemQuestionario EstiloItemQuestionario { get; set; }
        public virtual string Questao { get; set; }
        public virtual decimal? ValorQuestao { get; set; }
        public virtual decimal? ValorAribuido { get; set; }
        public virtual string Gabarito { get; set; }
        public virtual List<string> Resposta { get; set; }
        public virtual string Feedback { get; set; }
        public virtual bool? ExibeFeedback { get; set; }
        public virtual bool InAvaliaProfessor { get; set; }
        public virtual bool? RespostaObrigatoria { get; set; }

        public virtual List<DTOItemQuestionarioParticipacaoOpcoes> ListaOpcoes { get; set; }
        public virtual List<DTORespostaProfessor> ListaRespostaProfessor { get; set; }
    }

    public class DTORespostaProfessor
    {
        public virtual int ID { get; set; }
        public virtual int ProfessorId { get; set; }
        public virtual string Resposta { get; set; }

        public List<DTORespostaProfessorOpcao> ListaRespostaOpcoes { get; set; }
    }

    public class DTORespostaProfessorOpcao
    {
        public virtual int ID { get; set; }
        public virtual int OpcaoId { get; set; }
        public virtual bool RespostaSelecionada { get; set; }
    }
}
