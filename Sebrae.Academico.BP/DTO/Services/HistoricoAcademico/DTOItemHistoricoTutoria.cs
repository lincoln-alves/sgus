namespace Sebrae.Academico.BP.DTO.Services.HistoricoAcademico
{
    public class DTOItemHistoricoTutoria
    {
        public virtual string NomeSolucao { get; set; }
        public virtual int IdOferta { get; set; }
        public virtual int IdTurma { get; set; }
        public virtual string Instituicao { get; set; }
        public virtual string DataInicio { get; set; }
        public virtual string DataFim { get; set; }
        //public virtual int? IdMatricula { get; set; }
        //public virtual string Tipo { get; set; }
        public virtual bool TemCertificado { get; set; }
        //public virtual string CargaHoraria { get; set; }
        //public virtual string Situacao { get; set; }
        //public virtual bool QuestionarioPrePendente { get; set; }
        //public virtual bool QuestionarioPosPendente { get; set; }
        //public virtual int? IdTrilhaNivel { get; set; }
        //public virtual string LKCertificado { get; set; }
        //public virtual int idExtraCurricular { get; set; }
        //public virtual string LKAcesso { get; set; }
    }
}
