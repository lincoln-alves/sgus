using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class TrilhaTopicoTematicoDTO
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string AtividadeFormativaLabelTexto { get; set; }
        public virtual string AtividadeFormativaLabelArquivo { get; set; }
        public virtual double PercentualConclusaoUC { get; set; }
        public virtual double PercentualConclusaoAI { get; set; }

        public virtual bool HabilitaSolucaoUC { get; set; }
        public virtual bool HabilitaSolucaoAI { get; set; }
        public virtual bool HabilitaCadastroSolucaoAI { get; set; }
        public virtual bool HabilitaCadastroAtividadeFormativa { get; set; }
        public virtual bool TemEstrelaUC { get; set; }
        public virtual bool TemEstrelaAI { get; set; }
        public virtual string AtividadeFormativaArquivoInstrucoes { get; set; }

        public virtual int QuantidadeNotificacoesUCTopico { get; set; }
        public virtual int QuantidadeNotificacoesAITopico { get; set; }
        public virtual int QuantidadeNotificacoesAtividadeFormativa { get; set; }

        public virtual List<ItemTrilhaDTO> ListaItemTrilhaUC { get; set; }
        public virtual List<ItemTrilhaDTO> ListaItemTrilhaAI { get; set; }
        public virtual List<AtividadeFormativaDTO> ListaAtividadeFormativa { get; set; }


        public virtual bool HabilitaBotaoEnviarMensagem { get; set; }
        public virtual bool HabilitaBotaoEnviarParticipacao { get; set; }
        public virtual bool HabilitaFormularioParticipacao { get; set; }
        public virtual bool TemParticipacao { get; set; }
        public virtual bool ParticipacaoAprovada { get; set; }
        public virtual string StatusParticipacao { get; set; }

        public TrilhaTopicoTematicoDTO()
        {
            ListaItemTrilhaUC = new List<ItemTrilhaDTO>();
            ListaItemTrilhaAI = new List<ItemTrilhaDTO>();
            ListaAtividadeFormativa = new List<AtividadeFormativaDTO>();
            HabilitaSolucaoUC = false;
            HabilitaSolucaoAI = false;
            HabilitaCadastroSolucaoAI = false;
            HabilitaCadastroAtividadeFormativa = false;
            
            HabilitaBotaoEnviarMensagem = false;
            HabilitaBotaoEnviarParticipacao = true;
            HabilitaFormularioParticipacao = true;
            TemParticipacao = false;
            ParticipacaoAprovada = false;
        
        }
    }
}
