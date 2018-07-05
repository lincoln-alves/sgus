using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class QuestaoResposta
    {
        public virtual int ID { get; set; }
        public virtual enumDominio? Dominio { get; set; }
        public virtual string Comentario { get; set; }
        public virtual MatriculaTurma MatriculaTurma { get; set; }
        public virtual StatusMatricula StatusMatricula { get; set; }
        public virtual Questao Questao { get; set; }

        public virtual IEnumerable<Avaliacao> Avaliacoes { get; set; }

        public virtual bool IsRespondido()
        {
            switch (Questao.Tipo)
            {
                case enumTipoQuestao.Dominio:
                    return Dominio != null;
                case enumTipoQuestao.Dissertativa:
                    // Questões dissertativas não são obrigatórias.
                    return true;
                case enumTipoQuestao.Resultado:
                    return StatusMatricula != null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
