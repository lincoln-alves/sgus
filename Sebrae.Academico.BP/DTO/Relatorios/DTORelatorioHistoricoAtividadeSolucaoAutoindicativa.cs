
namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioHistoricoAtividadeSolucoesAutoindicativa
    {
        //public int IDTopicoTematico { get; set; }
        //public string TopicoTematico { get; set; }
        //public string TituloSolucao { get; set; }
        //public string RegistroAprendizagem { get; set; }
        

        public int IDTopicoTematico { get; set; }
        public string NomeSolucao { get; set; }
        public string Objetivo { get; set; }

        public virtual bool IsAutoIndicativa { get; set; }
        public virtual bool TemParticipacaoNaSolucao { get; set; }
        public virtual bool TemParticipacaoNoTopico { get; set; }

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

    }
}
