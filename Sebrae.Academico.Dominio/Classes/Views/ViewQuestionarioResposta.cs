

namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewQuestionarioResposta
    {
        public virtual int ID { get; set; }
        public virtual int ID_Questionario { get; set; }
        public virtual int ID_Turma { get; set; }
        public virtual int ID_ItemQuestionarioParticipacao { get; set; }
        public virtual string NM_Opcao { get; set; }
    }
}
