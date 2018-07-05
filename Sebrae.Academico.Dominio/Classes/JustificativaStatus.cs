namespace Sebrae.Academico.Dominio.Classes
{
    public class JustificativaStatus
    {
        public JustificativaStatus()
        {
        }

        public JustificativaStatus(Turma turma)
        {
            Turma = turma;
        }

        public virtual int ID { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Turma Turma { get; set; }
    }
}
