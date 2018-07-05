using System.Collections.Generic;
using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOItemMeusCursos : DTOEntidadeBasica
    {
        public DTOItemMeusCursos()
        {
            this.LinkSemAcesso = new List<DTOLinkSemAcesso>();
            this.CapacitacoesPrograma = new List<DTOCapacitacao>();
        }

        public virtual string NomeSolucao { get; set; }
        public virtual string Fornecedor { get; set; }
        public virtual string DataInicio { get; set; }
        public virtual string DataLimite { get; set; }
        public virtual int? IdMatricula { get; set; }
        public virtual int? IdTurma { get; set; }
        public virtual int? IdTrilhaNivel { get; set; }
        public virtual int? IdTrilha { get; set; }
        public virtual int? IdProgramaPortal { get; set; }
        public virtual string Tipo { get; set; }
        public virtual string CargaHoraria { get; set; }
        public virtual int? PrazoEmDias { get; set; }
        public virtual string LinkAcesso { get; set; }
        public virtual string Link { get; set; }
        public virtual bool HabilitaCancelamento { get; set; }
        public virtual bool QuestionarioCancelamentoPendente { get; set; }
        public virtual bool QuestionarioRespondido { get; set; }
        public virtual bool QuestionarioPrePendente { get; set; }
        public virtual bool QuestionarioPosPendente { get; set; }
        public virtual List<DTOLinkSemAcesso> LinkSemAcesso { get; set; }
        public virtual int SituacaoID { get; set; }
        public virtual string Situacao { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual int? IDNode { get; set; }
        public virtual int? PorcentagemConlusaoPrograma { get; set; }
        public virtual string TextoConclusaoPrograma { get; set; }
        public virtual string TextoApresentacao { get; set; }
        public virtual List<DTOCapacitacao> CapacitacoesPrograma { get; set; }
        public virtual bool QuestionarioEficaciaPendente { get; set; }
        public virtual int QuantidadeItensQuestionarioAgrupados { get; set; }
        public int IdOferta { get;  set; }
    }

    public class DTOLinkSemAcesso
    {
        public virtual string MotivoLinkSemAcesso { get; set; }
    }

    public class DTOCapacitacao
    {

        public DTOCapacitacao()
        {
            this.ModulosCapacitacao = new List<DTOModulo>();
            this.Programa = new DTOPrograma();
            this.TurmaCapacitacao = new List<DTOTurmaCapacitacao>();
        }

        public virtual bool PodeRealizarIscricao { get; set; }
        public virtual string NomeCapacitacao { get; set; }
        public virtual int? PorcentagemConclusaoCapacitacao { get; set; }
        public virtual string TextoConclusaoCapacitacao { get; set; }
        public virtual bool HabilitaCancelamento { get; set; }
        public virtual int ID { get; set; }
        public virtual string DataInicio { get; set; }
        public virtual string DataFim { get; set; }
        public virtual string DataInicioInscricoes { get; set; }
        public virtual string DataFimInscricoes { get; set; }
        public virtual string descricao { get; set; }
        public virtual bool jaInscrito { get; set; }

        //public virtual int TotalSolucoes { get; set; }
        //public virtual int TotalAprovacoesSolucoes { get; set; }

        public int idMatricula { get; set; }

        public virtual List<DTOTurmaCapacitacao> TurmaCapacitacao { get; set; }
        public virtual List<DTOModulo> ModulosCapacitacao { get; set; }
        public virtual DTOPrograma Programa { get; set; }
    }


    public class DTOModulo
    {
        public DTOModulo()
        {
            this.SolucoesModulo = new List<DTOItemMeusCursos>();
        }
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual List<DTOItemMeusCursos> SolucoesModulo { get; set; }
        public virtual bool PreRequisitoPendente { get; set; }
        public virtual string DataInicio { get; set; }
        public virtual string DataFim { get; set; }
        public virtual int? PorcentagemConclusaoModulo { get; set; }
        public virtual string TextoConclusaoModulo { get; set; }
    }

}