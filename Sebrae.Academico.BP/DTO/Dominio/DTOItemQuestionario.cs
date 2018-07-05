using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOItemQuestionario
    {
        public int ID { get; set; }
        public virtual string Questao { get; set; }
        public virtual int IdQuestionario { get; set; }
        public virtual DTOQuestionario Questionario { get; set; }
        public virtual DTOItemQuestionario QuestionarioEnunciado { get; set; }
        public virtual DTOTipoItemQuestionario TipoItemQuestionario { get; set; }
        public virtual DTOEstiloItemQuestionario EstiloItemQuestionario { get; set; }
        public virtual decimal ValorQuestao { get; set; }
        public virtual string NomeGabarito { get; set; }
        public virtual IList<DTOItemQuestionarioOpcoes> ListaItemQuestionarioOpcoes { get; set; }
        public virtual string Feedback { get; set; }
        public virtual string Comentario { get; set; }
        public virtual int? Ordem { get; set; }
        public virtual bool InAvaliaProfessor { get; set; }
        public virtual bool? ExibeFeedback { get; set; }
        public virtual bool? RespostaObrigatoria { get; set; }
    }

}