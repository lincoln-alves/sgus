using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.DTO.Relatorios
{
    public class DTORelatorioUsuarioPagante
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Matricula { get; set; }
        public string Situacao { get; set; }
        public string Unidade { get; set; }
        public string TelResidencial { get; set; }
        public string TelCelular { get; set; }
        public string UF { get; set; }
        public string NivelOcupacional { get; set; }


        public enumFormaPagamento FormaPagamento { get; set; }
        public bool Pago { get; set; }
        public bool PagamentoInformado { get; set; }
        public bool PagamentoConfirmado { get; set; }
        public DateTime? DataUltimaAtualizacao { get; set; }

        public string DescricaoPagamentoConfirmado { get { return PagamentoConfirmado ? "Sim" : "Não"; } }
        public string DescricaoPago { get { return Pago ? "Sim" : "Não"; } }
        public string DescricaoPagamentoInformado { get { return PagamentoInformado ? "Sim" : "Não"; } }
        

        public string DescricaoFormaPagamento
        {
            get
            {
                switch (FormaPagamento)
                {
                    case enumFormaPagamento.Boleto:
                        return "Boleto";
                    case enumFormaPagamento.DebitoEmConta:
                        return "Débito em Conta";
                    default:
                        return "-";
                }
            }
        }
    }

    public class DTORelatorioUsuarioPaganteDescricoes : DTORelatorioUsuarioPagante
    {
    }
}
