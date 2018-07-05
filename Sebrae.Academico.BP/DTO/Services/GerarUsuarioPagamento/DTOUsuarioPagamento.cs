using System;

namespace Sebrae.Academico.BP.DTO.Services
{
    public class DTOUsuarioPagamento
    {
        public virtual int IdUsuario { get; set; }
        public virtual int IdConfiguracaoPagamento { get; set; }
        public virtual DateTime DataInicioVigencia { get; set; }
        public virtual DateTime DataFimVigencia { get; set; }
        public virtual DateTime? DataPagamento { get; set; }
        public virtual decimal ValorPagamento { get; set; }
        public virtual DateTime DataInicioRenovacao { get; set; }
        public virtual DateTime DataMaxInadimplencia { get; set; }
        public virtual string CodigoPagamento { get; set; }
        public virtual bool PagamentoEfetuado { get; set; }
        public virtual DateTime? DataAceiteTermoAdesao { get; set; }
        public virtual DateTime? DataVencimento { get; set; }
        public virtual string FormaPagamento { get; set; }
        public virtual string IdConvenio { get; set; }
        public virtual string QtdPontos { get; set; }
        public virtual string MensagemBoleto { get; set; }
        public virtual string CPF { get; set; }
    }

}

