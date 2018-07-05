using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMConfiguracaoPagamento : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<ConfiguracaoPagamento> repositorio;

        public BMConfiguracaoPagamento()
        {
            repositorio = new RepositorioBase<ConfiguracaoPagamento>();
        }

        public ConfiguracaoPagamento ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IQueryable<ConfiguracaoPagamento> ObterTodosIQueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public IList<ConfiguracaoPagamento> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public void Salvar(ConfiguracaoPagamento pConfiguracao)
        {
            ValidarConfiguracaoPagamentoInformado(pConfiguracao);
            repositorio.Salvar(pConfiguracao);
        }

        private void ValidarConfiguracaoPagamentoInformado(ConfiguracaoPagamento pConfiguracao)
        {
            ValidarInstancia(pConfiguracao);

            if (pConfiguracao.TipoPagamento == null || (pConfiguracao.TipoPagamento != null && pConfiguracao.TipoPagamento.ID == 0))
            {
                throw new AcademicoException("Tipo de Pagamento não informado. Campo Obrigatório");
            }

            if (string.IsNullOrWhiteSpace(pConfiguracao.Nome))
            {
                throw new AcademicoException("Nome. Campo Obrigatório!");
            }

            if (pConfiguracao.QuantidadeDiasPagamento <= 0)
            {
                throw new AcademicoException("Qtd. Dias Para Pagamento. Campo Obrigatório");
            }

            if (string.IsNullOrWhiteSpace(pConfiguracao.TextoTermoAdesao))
            {
                throw new AcademicoException("Termo de Adesão. Campo Obrigatório");
            }

            if (pConfiguracao.ID == 0)
            {

                //Verifica se o nome ja existe
                bool existeRegistroCadastrado = this.VerificarExistenciaDaConfiguracaoPagamento(pConfiguracao.Nome);

                if (existeRegistroCadastrado)
                {
                    throw new AcademicoException(string.Format("A Configuração Pagamento {0} já existe",
                                                 pConfiguracao.Nome));
                }
            }
        }

        private bool VerificarExistenciaDaConfiguracaoPagamento(string pNomeConfiguracaoPagamento)
        {
            bool existeRegistro = false;
            var query = repositorio.session.Query<ConfiguracaoPagamento>();
            existeRegistro = query.Any(x => x.Nome.Trim().ToUpper() == pNomeConfiguracaoPagamento.Trim().ToUpper());
            return existeRegistro;
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IList<ConfiguracaoPagamento> ObterPorFiltro(ConfiguracaoPagamento configuracaoPagamento)
        {
            var query = repositorio.session.Query<ConfiguracaoPagamento>();

            if (configuracaoPagamento.TipoPagamento != null)
                query = query.Where(x => x.TipoPagamento.ID == configuracaoPagamento.TipoPagamento.ID);

            return query.ToList();
        }

        public void Excluir(ConfiguracaoPagamento configuracaoPagamento)
        {
            if (this.ValidarDependencias(configuracaoPagamento))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes.");

            repositorio.Excluir(configuracaoPagamento);
        }

        protected virtual bool ValidarDependencias(object pParametro)
        {
            ConfiguracaoPagamento configuracaoPagamento = (ConfiguracaoPagamento)pParametro;

            return (configuracaoPagamento.ListaPagamento != null && configuracaoPagamento.ListaPagamento.Count > 0);
        }

        public IList<ConfiguracaoPagamento> ObterHistoricoDePagamento(int idConfiguracaoPagamento)
        {

            IList<ConfiguracaoPagamento> ListaConfiguracaoPagamento = null;

            var query = repositorio.session.Query<ConfiguracaoPagamento>();
            query = query.Where(x => x.ID == idConfiguracaoPagamento);
            ListaConfiguracaoPagamento = query.ToList<ConfiguracaoPagamento>();
            return ListaConfiguracaoPagamento;

        }

        public bool VerificarArquivoDeCobrancaEletronicaRcb001(string nomeArquivo, string dataEntrada)
        {
            bool ArquivoOk = false;

            DateTime Data;
            if (nomeArquivo.Length == 0)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "O arquivo é obrigatório.");
            }

            if (dataEntrada.Length == 0)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Entre com a data desejada para o processamento, normalmente será o dia anterior.");
            }

            Data = CommonHelper.TratarData(dataEntrada, "Data de Entrada").Value;

            ArquivoOk = true;
            return ArquivoOk;
        }

        public bool VerificarDataArquivoDeCobrancaEletronicaRcb001(string DataEntrada)
        {
            bool ArquivoOk = false;

            try
            {
                DateTime Data;

                Data = CommonHelper.TratarData(DataEntrada, "Data de Entrada").Value;

                ArquivoOk = true;
                return ArquivoOk;

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return ArquivoOk;
            }

        }




    }
}
