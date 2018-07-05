
using System.Collections.Generic;
namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class ItemTrilhaDTO
    {
        public virtual int ID { get; set; }
        public virtual string Nome { get; set; }
        public virtual string FormaAquisicao { get; set; }
        public virtual string FormaAquisicaoImagem { get; set; }
        public virtual int QuantidadePontosParticipacao { get; set; }
        public virtual string Objetivo { get; set; }
        public virtual int? IDObjetivo { get; set; }
        public virtual string NomeIndicador { get; set; }
        public virtual string EmailIndicador { get; set; }
        public virtual string CaminhoAnexo { get; set; }
        public virtual string CargaHoraria { get; set; }
        public virtual string ReferenciaBibliografica { get; set; }
        public virtual string Local { get; set; }
        public virtual string LinkConteudo { get; set; }
        public virtual bool Aprovado { get; set; }
        public virtual string AprovadoTexto { get; set; }
        public virtual SolucaoEducacionalDTO SolucaoEducacional { get; set; }
        public virtual List<ItemTrilhaParticipacaoDTO> ListaItemTrilhaParticipacao { get; set; }
        public virtual string StatusAprovacao { get; set; }
        public virtual int QuantidadeNotificacoesUCItem { get; set; }
        public virtual int QuantidadeNotificacoesAIItem { get; set; }


        public virtual bool HabilitaBotaoEnviarMensagem { get; set; }
        public virtual bool HabilitaBotaoEnviarParticipacao { get; set; }
        public virtual bool HabilitaFormularioParticipacao { get; set; }
        public virtual bool TemParticipacao { get; set; }
        public virtual bool ParticipacaoAprovada { get; set; }
        public virtual string StatusParticipacao { get; set; }

        /// <summary>
        /// Informa se a solução é obrigatória para inscrição no próximo nível da trilha.
        /// </summary>
        public virtual bool SolucaoObrigatoria { get; set; }


        public ItemTrilhaDTO()
        {
            SolucaoEducacional = new SolucaoEducacionalDTO();
            ListaItemTrilhaParticipacao = new List<ItemTrilhaParticipacaoDTO>();

            HabilitaBotaoEnviarMensagem = false;
            HabilitaBotaoEnviarParticipacao = false;
            HabilitaFormularioParticipacao = false;
            TemParticipacao = false;
            ParticipacaoAprovada = false;
        }
    }
}
