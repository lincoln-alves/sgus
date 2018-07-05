using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class QuestionarioAssociacaoEnvio : EntidadeBasica
    {
        public virtual QuestionarioAssociacao QuestionarioAssociacao { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual DateTime DataEnvio { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
