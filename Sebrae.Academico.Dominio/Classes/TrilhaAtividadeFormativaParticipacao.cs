using System;
using System.Collections.Generic;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class TrilhaAtividadeFormativaParticipacao: EntidadeBasicaPorId
    {
        public virtual string TextoParticipacao { get; set; }
        public virtual DateTime DataEnvio { get; set; }
        public virtual DateTime? DataPrazoAvaliacao { get; set; }
        //public virtual string Comentario { get; set; }
        //public virtual bool HabilitaReenvio { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual TrilhaTopicoTematico TrilhaTopicoTematico { get; set; }
        public virtual FileServer FileServer { get; set; }
        public virtual Usuario Monitor { get; set; }
        public virtual enumTipoParticipacaoTrilha TipoParticipacao { get; set; } 

        public virtual bool? Autorizado { get; set; }
        public virtual bool? Visualizado { get; set; }
        
        public TrilhaAtividadeFormativaParticipacao()
        {
            this.FileServer = new FileServer();
        }



        public virtual string AutorizadoFormatado
        {
            get
            {
                if (Autorizado.HasValue)
                {
                    if (Autorizado.Value)
                        return "Aprovado";

                    return "Em revisão";
                }

                return "Pendente";
            }
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
