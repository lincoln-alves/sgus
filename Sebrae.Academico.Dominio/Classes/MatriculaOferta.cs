using System;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Collections.Generic;
using System.Linq;


namespace Sebrae.Academico.Dominio.Classes
{
    public class MatriculaOferta : EntidadeBasicaComStatus
    {
        public virtual Oferta Oferta { get; set; }
        public virtual DateTime DataSolicitacao { get; set; }

        public virtual Uf UF { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }
        public virtual string CDCertificado { get; set; }
        public virtual DateTime? DataGeracaoCertificado { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual IList<MatriculaTurma> MatriculaTurma { get; set; }
        public virtual DateTime? DataStatusMatricula { get; set; }
        public virtual string LinkCertificado { get; set; }
        public virtual string LinkAcesso { get; set; }
        public virtual bool FornecedorNotificado { get; set; }
        public virtual int? CertificadoEmitidoPorGestor { get; set; }
        public virtual IList<QuestaoResposta> QuestoesRespostas { get; set; }
        public virtual IList<ItemTrilhaParticipacao> ListaItemTrilhaParticipacao { get; set; }

        public virtual string NomeSolucaoEducacional
        {
            get
            {
                if (this.Oferta != null && this.Oferta.SolucaoEducacional != null)
                    return this.Oferta.SolucaoEducacional.Nome;
                else
                    return string.Empty;
            }
            
        }

        public virtual string NomeTurma
        {
            get
            {
                if (this.MatriculaTurma != null && MatriculaTurma.Count > 0)
                    return this.MatriculaTurma.FirstOrDefault().Turma.Nome;
                else
                    return string.Empty;
            }

        }

        public virtual string DataConclusao
        {
            get
            {
                if (MatriculaTurma != null && MatriculaTurma.Count > 0)
                {
                    var matriculaTurma = MatriculaTurma.FirstOrDefault();

                    return matriculaTurma != null && matriculaTurma.DataTermino.HasValue
                        ? matriculaTurma.DataTermino.Value.ToString("dd/MM/yyyy")
                        : string.Empty;
                }

                return string.Empty;
            }
        }

        public virtual string NotaFinal
        {
            get
            {
                if (IsOuvinte())
                    return null;

                if (MatriculaTurma != null && MatriculaTurma.Count > 0)
                {
                    var matriculaTurma = MatriculaTurma.FirstOrDefault();

                    return matriculaTurma != null && matriculaTurma.MediaFinal.HasValue &&
                           matriculaTurma.DataTermino.HasValue
                        ? matriculaTurma.MediaFinal.ToString()
                        : string.Empty;
                }

                return string.Empty;
            }
        }

        public virtual bool IsUtilizado()
        {
            return IsCancelado() || StatusMatricula == enumStatusMatricula.FilaEspera || StatusMatricula == enumStatusMatricula.Ouvinte;
        }

        public virtual bool IsCancelado()
        {
            return StatusMatricula == enumStatusMatricula.CanceladoAdm ||
                   StatusMatricula == enumStatusMatricula.CanceladoAluno ||
                   StatusMatricula == enumStatusMatricula.CanceladoGestor ||
                   StatusMatricula == enumStatusMatricula.CanceladoTurma;
        }

        public virtual bool IsAbandono()
        {
            return StatusMatricula == enumStatusMatricula.Abandono;
        }

        public virtual bool IsAprovado()
        {
            var lista = new List<enumStatusMatricula>
            {
                enumStatusMatricula.Aprovado,
                enumStatusMatricula.AprovadoComoConsultor,
                enumStatusMatricula.AprovadoComoConsultorComAcompanhamento,
                enumStatusMatricula.AprovadoComoFacilitador,
                enumStatusMatricula.AprovadoComoFacilitadorComAcompanhamento,
                enumStatusMatricula.AprovadoComoFacilitadorConsultor,
                enumStatusMatricula.AprovadoComoGestor,
                enumStatusMatricula.AprovadoComoModerador,
                enumStatusMatricula.AprovadoComoModeradorComAcompanhamento,
                enumStatusMatricula.AprovadoComoMultiplicador,
                enumStatusMatricula.AprovadoComoMultiplicadorComAcompanhamento,
                enumStatusMatricula.Concluido,
                enumStatusMatricula.Inscrito
            };

            return lista.Contains(StatusMatricula);
        }

        public virtual bool IsReprovado()
        {
            return StatusMatricula == enumStatusMatricula.Reprovado;
        }

        public virtual bool IsOuvinte()
        {
            return StatusMatricula == enumStatusMatricula.Ouvinte;
        }

        public virtual bool IsDesistencia()
        {
            return StatusMatricula == enumStatusMatricula.Abandono;
        }

    }
}
