using System;
using Sebrae.Academico.Dominio.Enumeracao;
namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações de Configuração de Pagamento
    /// </summary>
    public class DTOAlunoDaTrilha
    {

        #region "Informações do Tópico Temático"

        public virtual int IdTopicoTematico { get; set; }
        public virtual string TopicoTematico { get; set; }

        #endregion

        #region "Informações do Item da Trilha"

        public virtual string NomeItemTrilha { get; set; }

        #endregion

        public virtual bool IsAutoIndicativa { get; set; }
        public virtual bool TemParticipacaoNaSolucao { get; set; }

        public virtual string StatusAtividadeFormativa
        {
            get
            {
                string statusAtividadeFormativa = string.Empty;
                if (this.TemParticipacaoNoTopico)
                {
                    statusAtividadeFormativa = "Concluído";
                }
                else if (!this.TemParticipacaoNoTopico)
                {
                    statusAtividadeFormativa = "Pendente";
                }

                return statusAtividadeFormativa;
            }
        }

        public virtual string TemParticipacaoNaSolucaoString
        {
            get
            {
                string temParticipacaoNaSolucaoString = string.Empty;
                if (this.TemParticipacaoNaSolucao)
                {
                    temParticipacaoNaSolucaoString = "Sim";
                }
                else
                {
                    temParticipacaoNaSolucaoString = "Não";
                }

                return temParticipacaoNaSolucaoString;
            }
        }

        public virtual bool TemParticipacaoNoTopico { get; set; }

        public virtual string TemParticipacaoNoTopicoString
        {
            get
            {
                string temParticipacaoNoTopicoString = string.Empty;
                if (this.TemParticipacaoNaSolucao)
                {
                    temParticipacaoNoTopicoString = "Sim";
                }
                else
                {
                    temParticipacaoNoTopicoString = "Não";
                }

                return temParticipacaoNoTopicoString;
            }
        }

        public virtual string Objetivo { get; set; }


        //

        public string Trilha { get; set; }
        public string Nivel { get; set; }
        public string StatusMatricula { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataLimite { get; set; }

        //
    }
}
