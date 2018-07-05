

using System;

namespace Sebrae.Academico.BP.DTO.Services.HistoricoAcademico
{
    public class DTOItemHistoricoAcademico
    {
        public virtual string NomeSolucao { get; set; }
        public virtual string Instituicao { get; set; }
        public virtual string DataInicio { get; set; }
        public virtual string DataFim { get; set; }
        public virtual int? IdMatricula { get; set; }
        public virtual string Tipo { get; set; }
        public virtual bool TemCertificado { get; set; }
        public virtual string CargaHoraria { get; set; }
        public virtual string Situacao { get; set; }
        public virtual bool QuestionarioPrePendente { get; set; }
        public virtual bool QuestionarioPosPendente { get; set; }
        public virtual bool QuestionarioEficaciaPendente { get; set; }
        public virtual int? IdTurma { get; set; }
        public virtual int? IdTrilhaNivel { get; set; }
        public virtual string LKCertificado { get; set; }
        public virtual int idExtraCurricular { get; set; }
        public virtual string LKAcesso { get; set; }
        public virtual string Feedback { get; set; }
        public virtual int? QuantidadeItensQuestionarioAgrupados { get; set; }
        public DTOItemHistoricoAcademico()
        {
            CargaHoraria = string.Empty;
            Situacao = string.Empty;
        }
        public virtual int? IdTurmaQuestionarioCancelamento { get; set; }
        public virtual int? IdTurmaQuestionarioAbandono { get; set; }
        public virtual DateTime? DataDisparoLinkPesquisa { get; set; }
        public virtual DateTime DataDisparoLinkEficacia { get; set; }
    }
}
