using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Dominio;


namespace Sebrae.Academico.BP.DTO.Services.Questionario
{
    public class DTOQuestionarioParticipacao: DTOEntidadeBasicaPorId, IRetornoWebService
    {
        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime? DataParticipacao { get; set; }
        public virtual DateTime? DataLimiteParticipacao { get; set; }
        public virtual string TextoEnunciadoPre { get; set; }
        public virtual string TextoEnunciadoPos { get; set; }
        public virtual DTOTipoQuestionarioAssociacao TipoQuestionarioAssociacao { get; set; }
        public virtual List<DTOItemQuestionarioParticipacao> ListaItemQuestionarioParticipacao { get; set; }
        public virtual DTOQuestionario Questionario { get; set; }
        public virtual List<DTOProfessor> ListaProfessor { get; set; }
        public virtual int? IdTurma { get; set; }
        public int TipoQuestionario { get; set; }
        public string Mensagem { get; set; }
        public int Erro { get; set; }
    }
}
