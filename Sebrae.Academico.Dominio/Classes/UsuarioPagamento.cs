using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioPagamento: EntidadeBasicaPorId
    {
        public virtual Usuario Usuario { get; set; }
        public virtual ConfiguracaoPagamento ConfiguracaoPagamento { get; set; }
        public virtual DateTime DataInicioVigencia { get; set; }
        public virtual DateTime DataFimVigencia { get; set; }
        public virtual DateTime? DataPagamento { get; set; }
        public virtual decimal ValorPagamento { get; set; }
        public virtual DateTime DataInicioRenovacao { get; set; }
        public virtual DateTime DataMaxInadimplencia { get; set; }
        public virtual string NossoNumero { get; set; }

        /// <summary>
        /// Sistema considera que o pagamento foi realizado.
        /// </summary>
        public virtual bool PagamentoEfetuado { get; set; }
        public virtual DateTime? DataAceiteTermoAdesao { get; set; }
        public virtual DateTime? DataVencimento { get; set; }
        public virtual enumFormaPagamento FormaPagamento { get; set; }

        public virtual string TextoDaAutenticacaoBancaria { get; set; }
        
        /// <summary>
        /// Usuário que informou o pagamento pelo site
        /// </summary>
        public virtual bool? PagamentoInformado { get; set; }
        public virtual DateTime? DataPagamentoInformado { get; set; }
        
        /// <summary>
        /// Acontece quando o pagamento é confirmado pelo arquivo de retorno do banco
        /// </summary>
        public virtual bool? PagamentoConfirmado { get; set; }
        public virtual bool PagamentoEnviadoBanco { get; set; }

        public virtual DateTime DataFimVigenciaCalculada { get; set; }
        public virtual DateTime DataInicioRenovacaoCalculada { get; set; }
        public virtual DateTime DataMaxInadimplenciaCalculada { get; set; }
        public virtual DateTime DataVencimentoCalculada { get; set; }
        
        
    }
}
