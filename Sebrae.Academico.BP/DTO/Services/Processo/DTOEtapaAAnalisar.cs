using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOEtapaAAnalisar
    {
        /// <summary>
        /// Usuário demandante
        /// </summary>
        public virtual string Usuario { get; set; }

        public virtual string Email { get; set; }

        public virtual string Processo { get; set; }

        public virtual string Etapa { get; set; }

        public virtual string Unidade { get; set; }

        public virtual DateTime DataSolicitacao { get; set; }

        public virtual DateTime? DataAlteracaoProcessoResposta { get; set; }

        public virtual DateTime? DataUltimaAtualizacaoEtapaResposta { get; set; }

        public virtual DateTime? PrazoEncaminhamento { get; set; }

        public virtual int ID_Processo { get; set; }

        public virtual int ID_ProcessoResposta { get; set; }

        public virtual int ID_Etapa { get; set; }

        public virtual int ID_EtapaResposta { get; set; }

        public virtual int ID_Usuario { get; set; }

        public virtual enumPrazoEncaminhamentoDemanda? ObterStatusEncaminhamento(DateTime? prazo)
        {
            if (prazo == null)
                return null;

            if (prazo.Value.Date < DateTime.Now)
                return enumPrazoEncaminhamentoDemanda.ForaDoPrazo;

            if ((prazo.Value.Date - DateTime.Today).Days <= 2)
                return enumPrazoEncaminhamentoDemanda.AExpirar;

            return enumPrazoEncaminhamentoDemanda.NoPrazo;
        }
    }
}
