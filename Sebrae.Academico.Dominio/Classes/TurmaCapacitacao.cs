using System.Collections.Generic;
using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TurmaCapacitacao : EntidadeBasica
    {
        public virtual Capacitacao Capacitacao { get; set; }
        public virtual IList<TurmaCapacitacaoPermissao> ListaPermissao { get; set; }
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual IList<QuestionarioAssociacao> ListaQuestionarioAssociacao { get; set; }

        public TurmaCapacitacao()
        {
            Capacitacao = new Capacitacao();
            ListaPermissao = new List<TurmaCapacitacaoPermissao>();
        }
    }
}
