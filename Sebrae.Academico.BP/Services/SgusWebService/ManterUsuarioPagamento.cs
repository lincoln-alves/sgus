using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.GerarPagamento;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Text.RegularExpressions;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterUsuarioPagamento : BusinessProcessServicesBase
    {

        private BMUsuarioPagamento usuarioPagamentoBM = new BMUsuarioPagamento();

        /// <summary>
        /// Grava na tabela a informação que o usuário realizou o pagamento.
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="nossoNumero">É o campo nosso número no boleto</param>
        /// <param name="autenticacaoBancaria"></param>
        /// <param name="dataPagamentoInformado"></param>
        /// <param name="valorPagamentoInformado"></param>
        /// <returns></returns>
        public void RegistrarPagamentoInformado(string cpf, string nossoNumero, string autenticacaoBancaria,
                                                string dataPagamentoInformado, string valorPagamentoInformado)
        {

            UsuarioPagamento usuarioPagamento = this.ObterInformacoesDePagamentoDoUsuarioPorNossoNumero(nossoNumero);
            decimal valorPago = 0;

            //Se não encontrou o pagamento pelo nosso número, exibe mensagem de erro
            if ((usuarioPagamento == null || (usuarioPagamento != null && usuarioPagamento.ID == 0)))
            {
                throw new AcademicoException("Pagamento não encontrado");
            }
            else
            {

                //Se ainda não pagou, verifica se o valor a pagar confere com o valor que o usuário esta informando que pagou
                if (!usuarioPagamento.PagamentoEfetuado)
                {

                    valorPago = 0;
                    if (!decimal.TryParse(valorPagamentoInformado, out valorPago))
                    {
                        throw new AcademicoException(string.Format("O valor {0} é inválido para o pagamento", valorPagamentoInformado));
                    }
                    else
                    {
                        //usuarioPagamento.ValorPagamento -> Valor a pagar
                        //valor pago -> valor informado que foi pago
                        if (!usuarioPagamento.ValorPagamento.Equals(valorPago))
                        {
                            throw new AcademicoException(string.Format("O valor informado {0} é diferente do valor a ser pago. Valor a ser pago: {1}", valorPago,
                                                         usuarioPagamento.ValorPagamento));
                        }
                        else
                        {
                            /* O Valor a pagar é igual ao valor informado que foi pago, então registra (grava) 
                               as informações de pagamento */
                            if (string.IsNullOrWhiteSpace(cpf))
                            {
                                throw new AcademicoException("O CPF informado é inválido");
                            }

                            if (!usuarioPagamento.Usuario.CPF.Equals(Regex.Replace(cpf.Trim(), @"\D", "").ToString()))
                            {
                                throw new AcademicoException("O CPF informado não corresponde ao do boleto");
                            }

                            DateTime dataPagamentoInformadoAux = DateTime.Now;
                            if (!DateTime.TryParse(dataPagamentoInformado, out dataPagamentoInformadoAux))
                            {
                                throw new AcademicoException(string.Format("O valor {0} é inválido para a data informada para pagamento", dataPagamentoInformado));
                            }
                            else
                            {
                                //Se conseguiu fazer o parse na data de pagamento, verifica se o usuário informou a data igual a 01/01/1900
                                if (dataPagamentoInformadoAux.Equals(DateTime.MinValue))
                                {
                                    throw new AcademicoException(String.Format("O valor {0} é inválido para a data informada para pagamento",
                                        dataPagamentoInformadoAux));
                                }
                            }

                            this.usuarioPagamentoBM.RegistrarPagamentoInformado(usuarioPagamento, dataPagamentoInformadoAux, autenticacaoBancaria, valorPago);
                        }
                    }

                }
                else
                {
                    throw new AcademicoException("Pagamento não encontrado");
                }
            }
        }

        private UsuarioPagamento ObterInformacoesDePagamentoDoUsuarioPorNossoNumero(string refTranNossoNumero)
        {
            if (string.IsNullOrWhiteSpace(refTranNossoNumero))
            {
                throw new AcademicoException("refTranNossoNumero é obrigatório");
            }

            return new BMUsuarioPagamento().ObterInformacoesDePagamentoDoUsuarioPorNossoNumero(refTranNossoNumero);
        }


        public DTOUsuarioPagamento GerarPagamento(DTOGerarPagamento dadosPagamento, AuthenticationRequest autenticacao)
        {
            var usuario = new BMUsuario().ObterPorId(dadosPagamento.IDUsuario);

            //Se não tiver pagamento pendente, gera um novo pagamento
            var usuarioPagamento = ObterObjetoUsuarioPagamento(dadosPagamento.IDConfiguracaoPagamento,
                dadosPagamento.IDFormaPagamento, usuario);

            PreencherObjetoUsuarioComDadosDoDTOManterUsuario(usuario, dadosPagamento);

            new BP.ManterUsuarioPagamento().IncluirPagamentoDoUsuario(usuario, usuarioPagamento);

            var usuarioPagamentoDto = PreencherDTOUsuarioPagamento(usuarioPagamento);

            return usuarioPagamentoDto;
        }

        private void PreencherObjetoUsuarioComDadosDoDTOManterUsuario(Usuario usuario, DTOGerarPagamento dtoUsuario)
        {
            usuario.Cep = Regex.Replace(dtoUsuario.CEP.Trim(), @"\D", "").ToString();
            usuario.Endereco = dtoUsuario.Logradouro;
            usuario.Complemento = dtoUsuario.Complemento;
            usuario.Bairro = dtoUsuario.Bairro;
            usuario.Cidade = dtoUsuario.Cidade;
            usuario.Estado = dtoUsuario.Estado;
        }

        private UsuarioPagamento ObterObjetoUsuarioPagamento(int idConfiguracaoPagamento, int idFormaPagamento,
            Usuario usuario)
        {
            var configuracaoPagamento =
                new ManterConfiguracaoPagamento().ObterConfiguracaoPagamentoPorId(idConfiguracaoPagamento);

            var dataFimVigencia = DateTime.Today.AddDays(configuracaoPagamento.QuantidadeDiasValidade);

            var usuarioPagamento = new UsuarioPagamento
            {
                Usuario = usuario,
                ConfiguracaoPagamento = configuracaoPagamento,
                DataInicioVigencia = DateTime.Today,
                DataFimVigencia = dataFimVigencia,
                DataPagamento = null,
                ValorPagamento = configuracaoPagamento.ValorAPagar,
                DataInicioRenovacao = dataFimVigencia.AddDays(-configuracaoPagamento.QuantidadeDiasRenovacao),
                DataMaxInadimplencia = dataFimVigencia.AddDays(configuracaoPagamento.QuantidadeDiasInadimplencia),
                DataVencimento = DateTime.Today.AddDays(configuracaoPagamento.QuantidadeDiasPagamento),
                //CodigoPagamento  = Gerado no Salvar
                PagamentoEfetuado = false,
                DataAceiteTermoAdesao = DateTime.Now,
                FormaPagamento = (enumFormaPagamento) idFormaPagamento,
                PagamentoEnviadoBanco = false
            };

            PreencherInformacoesDeAuditoria(usuarioPagamento);

            return usuarioPagamento;
        }

        private DTOUsuarioPagamento PreencherDTOUsuarioPagamento(UsuarioPagamento usuarioPagamentoPendente)
        {
            var dtoUsuarioPagamento = new DTOUsuarioPagamento
            {
                IdConfiguracaoPagamento = usuarioPagamentoPendente.ConfiguracaoPagamento.ID,
                IdUsuario = usuarioPagamentoPendente.Usuario.ID,
                DataAceiteTermoAdesao = usuarioPagamentoPendente.DataAceiteTermoAdesao,
                DataInicioVigencia = usuarioPagamentoPendente.DataInicioVigencia,
                DataFimVigencia = usuarioPagamentoPendente.DataFimVigencia,
                DataPagamento = usuarioPagamentoPendente.DataPagamento,
                ValorPagamento = usuarioPagamentoPendente.ValorPagamento,
                DataInicioRenovacao = usuarioPagamentoPendente.DataInicioRenovacao,
                DataMaxInadimplencia = usuarioPagamentoPendente.DataMaxInadimplencia,
                CodigoPagamento = usuarioPagamentoPendente.NossoNumero,
                PagamentoEfetuado = usuarioPagamentoPendente.PagamentoEfetuado,
                IdConvenio = "3072072",
                QtdPontos = "0",
                MensagemBoleto = "Não Aceitar após o vencimento",
                DataVencimento = usuarioPagamentoPendente.DataVencimento,
                CPF = usuarioPagamentoPendente.Usuario.CPF
            };

            if (usuarioPagamentoPendente.PagamentoEnviadoBanco &&
                usuarioPagamentoPendente.FormaPagamento == enumFormaPagamento.Boleto)
            {
                //21 -> Segunda via do Boleto
                dtoUsuarioPagamento.FormaPagamento = "21";
            }
            else
            {
                dtoUsuarioPagamento.FormaPagamento = ((int) usuarioPagamentoPendente.FormaPagamento).ToString();
            }

            return dtoUsuarioPagamento;
        }
    }
}
