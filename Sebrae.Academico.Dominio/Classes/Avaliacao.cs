using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Avaliacao
    {
        public Avaliacao()
        {
            Status = enumStatusAvaliacao.AguardandoResposta;
        }

        public virtual int ID { get; set; }
        public virtual enumStatusAvaliacao Status { get; set; }
        public virtual Usuario Analista { get; set; }
        public virtual Turma Turma { get; set; }

        public virtual IEnumerable<QuestaoResposta> Respostas { get; set; }

        public virtual bool IsRespondido(int qtQuestoes, int qtMatriculas)
        {
            return Respostas.Count(x => x.IsRespondido()) == (qtQuestoes * qtMatriculas);
        }
    }
}
