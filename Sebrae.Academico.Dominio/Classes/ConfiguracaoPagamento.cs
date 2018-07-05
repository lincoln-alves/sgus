using System;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ConfiguracaoPagamento: EntidadeBasica
    {
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual DateTime DataInicioCompetencia { get; set; }
        public virtual DateTime DataFimCompetencia { get; set; }
        public virtual decimal ValorAPagar { get; set; }
        public virtual bool BloqueiaAcesso { get; set; }
        public virtual bool Recursiva { get; set; }
        public virtual int QuantidadeDiasValidade { get; set; }
        public virtual int QuantidadeDiasRenovacao { get; set; }
        public virtual int QuantidadeDiasInadimplencia { get; set; }
        public virtual int QuantidadeDiasPagamento { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual string TextoTermoAdesao { get; set; }


        public virtual IList<ConfiguracaoPagamentoPublicoAlvo> ListaConfiguracaoPagamentoPublicoAlvo { get; set; }
        public virtual IList<UsuarioPagamento> ListaPagamento { get; set; }

        public ConfiguracaoPagamento()
        {
            this.ListaConfiguracaoPagamentoPublicoAlvo = new List<ConfiguracaoPagamentoPublicoAlvo>();
        }
    }
}
