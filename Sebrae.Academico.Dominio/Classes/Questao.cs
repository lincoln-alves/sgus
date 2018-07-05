using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Questao
    {
        public virtual int ID { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Pergunta { get; set; }
        public virtual enumTipoQuestao Tipo { get; set; }
        public virtual IEnumerable<QuestaoResposta> Respostas { get; set; }
        public virtual int Ordem { get; set; }
    }
}
