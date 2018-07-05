using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Transactions;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMUsuarioPagamento : BusinessManagerBase
    {
        public RepositorioBase<UsuarioPagamento> repositorio;


        public BMUsuarioPagamento()
        {
            repositorio = new RepositorioBase<UsuarioPagamento>();
        }

        /// <summary>
        /// Obtém informações dos pagamentos de um usuário.
        /// </summary>
        /// <param name="pUsuario">Id do Usuário</param>
        /// <returns>Lista contendo Informações dos pagamentos de um usuário</returns>
        public IList<UsuarioPagamento> ObterInformacoesDePagamentoDoUsuario(int pIdUsuario)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();

            query = query.Where(x => x.Usuario.ID == pIdUsuario);

            return query.ToList<UsuarioPagamento>();
        }

        public UsuarioPagamento ObterInformacoesDePagamentoDoUsuarioPorNossoNumero(string refTranNossoNumero)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();
            UsuarioPagamento usuarioPagamento = query.FirstOrDefault(x => x.NossoNumero == refTranNossoNumero.Trim());
            return usuarioPagamento;

        }


        public Int64 ObterInformacoesDoUltimoPagamentoDoUsuarioPorNossoNumero()
        {
            var query = repositorio.session.Query<UsuarioPagamento>();
            Int64 codigoPagamento = Int64.Parse(query.Max(x => x.NossoNumero));
            return codigoPagamento;

        }

        public UsuarioPagamento ObterInformacoesDePagamentoDoUsuarioPorIdUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();
            UsuarioPagamento usuarioPagamento = query.FirstOrDefault(x => x.Usuario.ID == idUsuario);
            return usuarioPagamento;
        }

        public IList<UsuarioPagamento> ObterInformacoesDePagamentoDoUsuarioPorUsuario(int idUsuario)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();
            IList<UsuarioPagamento> listaUsuarioPagamento = query.Where(x => x.Usuario.ID == idUsuario).ToList<UsuarioPagamento>();
            return listaUsuarioPagamento;
        }

        /// <summary>
        /// Função Dinâmica para gerar o nosso número.
        /// </summary>
        /// <param name="usuarioPagamento">Informações sobre o pagamento inserido.</param>
        /// <returns>Nosso número gerado dinamicamente</returns>
        public string GerarNossoNumero(UsuarioPagamento usuarioPagamento)
        {
            const int limiteDeDigitos = 17;
            const string digitosIniciais = "3072072"; // 0000000568";

            var qtdDigitosCodigoPagamento = usuarioPagamento.ID.ToString().Length; //Ex: 3001
            var qtdDigitos = digitosIniciais.Length + qtdDigitosCodigoPagamento; //Ex: 7 + 4 = 11 
            var qtdDeZeroaGerar = limiteDeDigitos - qtdDigitos; //Ex: 17 - 11 = 4 -> Terá que gerar 4 zeros

            //Ex: 25750110000003001
            var nossoNumero = string.Concat(digitosIniciais, "0".PadRight(qtdDeZeroaGerar, '0'), usuarioPagamento.ID.ToString());

            return nossoNumero;
        }

        public void Salvar(UsuarioPagamento usuarioPagamento)
        {

            this.ValidarUsuarioPagamentoInformado(usuarioPagamento);

            //Insert
            if (usuarioPagamento.ID == 0)
            {

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    //Insere um novo pagamento
                    repositorio.SalvarSemCommit(usuarioPagamento);

                    //Monta o Nosso número - Início
                    string nossoNumero = this.GerarNossoNumero(usuarioPagamento);
                    usuarioPagamento.NossoNumero = nossoNumero;
                    //Monta o Nosso número - Fim

                    //Atualiza o pagamento com o nosso número gerado
                    repositorio.SalvarSemCommit(usuarioPagamento);

                    repositorio.Commit();
                    transaction.Complete();
                }
            }
            //Update
            else
            {
                repositorio.Salvar(usuarioPagamento);
            }
        }

        private void ValidarUsuarioPagamentoInformado(UsuarioPagamento usuarioPagamento)
        {
            this.ValidarInstancia(usuarioPagamento);

            if (!usuarioPagamento.DataVencimento.HasValue)
            {
                throw new AcademicoException("Data de Vencimento. Campo Obrigatório");
            }
        }

        

        //public bool VerificarSeUsuarioPossuiAlgumPagamentoVigente(int idConfiguracaoPagamento, Usuario usuario)
        //{
        //    var query = repositorio.session.Query<UsuarioPagamento>();

        //    bool usuarioPossuiAlgumPagamento = query.Any(x => x.Usuario.ListaUsuarioPagamento.Any(y => x.ConfiguracaoPagamento.ID == idConfiguracaoPagamento &&
        //                                                                                          y.DataInicioVigencia.Date <= DateTime.Today &&
        //                                                                                          y.DataFimVigencia.Date >= DateTime.Today &&
        //                                                                                          y.DataInicioRenovacao.Date > DateTime.Today));

        //    return usuarioPossuiAlgumPagamento;

        //}

        public IList<UsuarioPagamento> ListarPagamentosVigentesDoUsuario(int idConfiguracaoPagamento, Usuario usuario)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();

            IList<UsuarioPagamento> ListaPagamentosVigentesDoUsuario = query.Where(x => x.Usuario.ListaHistoricoPagamento.Any(y => x.ConfiguracaoPagamento.ID == idConfiguracaoPagamento &&
                                                                                                  y.DataInicioVigencia.Date <= DateTime.Today &&
                                                                                                  y.DataMaxInadimplencia.Date >= DateTime.Today &&
                                                                                                  (y.DataVencimento >= DateTime.Today || y.DataPagamento.HasValue) &&
                                                                                                  y.Usuario.ID == usuario.ID)).ToList<UsuarioPagamento>();

            return ListaPagamentosVigentesDoUsuario;

        }


        public UsuarioPagamento ObterInformacoesCalculadasDeConfiguracaoPagamento(ConfiguracaoPagamento configuracaoPagamento)
        {
            UsuarioPagamento usuarioPagamento = new UsuarioPagamento();
            this.CalcularInformacoes(configuracaoPagamento, usuarioPagamento);
            return usuarioPagamento;
        }

        private void CalcularInformacoes(ConfiguracaoPagamento configuracaoPagamento, UsuarioPagamento usuarioPagamento)
        {
            //Calcular data Fim de vigência
            usuarioPagamento.DataFimVigenciaCalculada = configuracaoPagamento.DataInicioCompetencia.AddDays(configuracaoPagamento.QuantidadeDiasValidade);

            //Data de Inicio da Renovacao
            usuarioPagamento.DataInicioRenovacaoCalculada = configuracaoPagamento.DataFimCompetencia.AddDays(-configuracaoPagamento.QuantidadeDiasRenovacao);

            //Data Máxima de Inadimplência
            usuarioPagamento.DataMaxInadimplenciaCalculada = configuracaoPagamento.DataFimCompetencia.AddDays(configuracaoPagamento.QuantidadeDiasInadimplencia);

            //Data Vencimento
            usuarioPagamento.DataVencimentoCalculada = configuracaoPagamento.DataInicioCompetencia.AddDays(configuracaoPagamento.QuantidadeDiasPagamento);

        }

        public string ObterDia(string dataInformada)
        {
            string dia = string.Empty;
            dia = dataInformada.Substring(0, 2);
            return dia;
        }

        public string ObterMes(string dataInformada)
        {
            string mes = string.Empty;
            mes = dataInformada.Substring(2, 2);
            return mes;
        }

        public string ObterAnoCom4Digitos(string dataInformada)
        {
            string ano = string.Empty;
            ano = dataInformada.Substring(4, 4);
            return ano;
        }

        public string ObterAnoCom2DigitosParaArquivoDeDebitoDoBancoDoBrasilCBR643(string dataInformada)
        {
            string ano = string.Empty;
            ano = dataInformada.Substring(6, 2);
            return ano;
        }


        public void RegistrarPagamentoInformado(UsuarioPagamento usuarioPagamento,
                                                DateTime dataPagamentoInformado,
                                                string textoAutenticacaoBancaria,
                                                decimal valorPago)
        {
            this.ValidarInstancia(usuarioPagamento);
            this.ValidarInformacoesSobrePagamento(dataPagamentoInformado, textoAutenticacaoBancaria);

            //Update
            if (usuarioPagamento.ID > 0)
            {
                //Atualiza informações sobre o pagamento
                usuarioPagamento.PagamentoInformado = true;
                usuarioPagamento.PagamentoConfirmado = false;
                usuarioPagamento.ValorPagamento = valorPago;
                usuarioPagamento.DataPagamentoInformado = dataPagamentoInformado;
                usuarioPagamento.TextoDaAutenticacaoBancaria = textoAutenticacaoBancaria;

                //Atualiza o pagamento
                repositorio.Salvar(usuarioPagamento);
            }
        }

        private void ValidarInformacoesSobrePagamento(DateTime dataPagamentoInformado, string textoAutenticacaoBancaria)
        {
            //Verifica se a data informada para o pagamento está válida
            if (dataPagamentoInformado.Equals(DateTime.MinValue))
            {
                throw new AcademicoException(string.Format("A Data informada para pagamento {0} está inválida",
                                             dataPagamentoInformado));
            }

            //Se não informou o texto de autenticacao, gera mensagem de erro
            if (string.IsNullOrWhiteSpace(textoAutenticacaoBancaria))
            {
                throw new AcademicoException("Autenticação Bancária. Campo Obrigatório");
            }
        }


        public IList<UsuarioPagamento> ObterInformacoesDePagamentoPorConfiguracaoPagamento(int idConfiguracaoPagamento)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();
            IList<UsuarioPagamento> ListaPagamentosDoUsuario = query.Where(x => x.ConfiguracaoPagamento.ID == idConfiguracaoPagamento).ToList<UsuarioPagamento>();
            return ListaPagamentosDoUsuario;
        }

        public UsuarioPagamento ObterInformacoesDePagamentoPorID(int pIdUsuarioPagamento)
        {
            var query = repositorio.session.Query<UsuarioPagamento>();
            UsuarioPagamento usuarioPagamento = query.FirstOrDefault(x => x.ID == pIdUsuarioPagamento);
            return usuarioPagamento;
        }
    }
}
