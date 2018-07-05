using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemTrilhaParticipacao : EntidadeBasicaPorId
    {
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual ItemTrilha ItemTrilha { get; set; }

        public virtual string TextoParticipacao { get; set; }
        public virtual string Orientacao { get; set; }
        public virtual DateTime DataEnvio { get; set; }
        public virtual DateTime? DataPrazoAvaliacao { get; set; }
        public virtual Usuario Monitor { get; set; }
        public virtual enumTipoParticipacaoTrilha TipoParticipacao { get; set; }

        /// <summary>
        /// Clone da Atividade Trilha relacionada a este questionário.
        /// </summary>
        public virtual QuestionarioParticipacao QuestionarioParticipacao { get; set; }

        /// <summary>
        /// Matrícula relacionada a este questionário.
        /// </summary>
        public virtual MatriculaOferta MatriculaOferta { get; set; }

        //public virtual string Comentario { get; set; }
        //public virtual bool? HabilitaReenvio { get; set; }

        public virtual FileServer FileServer { get; set; }

        public virtual bool? Autorizado { get; set; }
        public virtual bool? Visualizado { get; set; }

        public virtual DateTime? DataAvaliacao { get; set; }

        public ItemTrilhaParticipacao()
        {
            this.FileServer = new FileServer();
        }

        public virtual string AutorizadoFormatado
        {
            get
            {

                if (UsuarioTrilha.StatusMatricula == enumStatusMatricula.CanceladoAluno ||
                    UsuarioTrilha.StatusMatricula == enumStatusMatricula.CanceladoAdm ||
                    UsuarioTrilha.StatusMatricula == enumStatusMatricula.Abandono ||
                    UsuarioTrilha.StatusMatricula == enumStatusMatricula.Reprovado)
                    return "Suspenso";

                if (Autorizado.HasValue && Autorizado.Value)
                    return "Aprovado";

                if (Autorizado.HasValue && Autorizado.Value == false)
                    return "Em revisão";

                if (!Autorizado.HasValue)
                    return "Pendente";

                return "";
            }
        }

        public virtual string StatusFormatado
        {
            get { return UsuarioTrilha.StatusMatriculaFormatado; }
        }

        public virtual string VisualizadoFormatado
        {
            get
            {
                if (Visualizado.HasValue)
                {
                    if (Visualizado.Value)
                    {
                        return "Sim";
                    }
                    else
                    {
                        return "Não";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual string TipoParticipacaoFormatado
        {
            get
            {
                if (TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor)
                {
                    return "Mensagem";
                }
                else if (TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro)
                {
                    return "Participação";
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual string NomeMonitor
        {
            get
            {
                if (this.Monitor != null)
                {
                    return this.Monitor.Nome;
                }
                else
                {
                    return "";
                }
            }
        }

    }
}
