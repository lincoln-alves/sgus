using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO.Filtros;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.BM.Mapeamentos.Procedures;

namespace Sebrae.Academico.BP
{
    public class ManterConfiguracaoPagamento : BusinessProcessBase, IDisposable
    {

        #region "Atributos Privados"

        private BMConfiguracaoPagamento bmConfiguracaoPagamento = null;

        #endregion

        #region "Construtor"

        public ManterConfiguracaoPagamento()
            : base()
        {
            bmConfiguracaoPagamento = new BMConfiguracaoPagamento();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<TipoPagamento> ObterListaTipoPagamento()
        {

            IList<TipoPagamento> listaTipoPagamento = null;

            try
            {
                using (BMTipoPagamento tipoPagamentoBM = new BMTipoPagamento())
                {
                    listaTipoPagamento = tipoPagamentoBM.ObterTodos();
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaTipoPagamento;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<ConfiguracaoPagamento> PesquisaConfiguracaoPagamento(int pTipoPagamento)
        {

            TipoPagamento tipoPagamento = null;
            IList<ConfiguracaoPagamento> listaConfiguracaoPagamento = null;

            try
            {
                using (BMTipoPagamento tipoPagBM = new BMTipoPagamento())
                {
                    tipoPagamento = tipoPagBM.ObterPorID(pTipoPagamento);
                }

                using (BMConfiguracaoPagamento confPagBM = new BMConfiguracaoPagamento())
                {
                    listaConfiguracaoPagamento = confPagBM.ObterPorFiltro(new ConfiguracaoPagamento()
                                             {
                                                 TipoPagamento = tipoPagamento
                                             });
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return listaConfiguracaoPagamento;

        }

        public TipoPagamento ObterTipoPagamentoPorId(int pId)
        {
            return new BMTipoPagamento().ObterPorID(pId);
        }

        public IList<ConfiguracaoPagamento> ObterTodasConfiguracaoPagamento()
        {
            return new BMConfiguracaoPagamento().ObterTodos();
        }

        public void InserirConfiguracaoPagamento(ConfiguracaoPagamento configuracaoPagamento)
        {
            try
            {
                this.PreencherInformacoesDeAuditoria(configuracaoPagamento);
                bmConfiguracaoPagamento.Salvar(configuracaoPagamento);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarConfiguracaoPagamento(ConfiguracaoPagamento configuracaoPagamento)
        {
            try
            {
                bmConfiguracaoPagamento.Salvar(configuracaoPagamento);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void ExcluirConfiguracaoPagamento(int IdConfiguracaoPagamento)
        {
            try
            {
                ConfiguracaoPagamento configuracaoPagamento = null;
                BMConfiguracaoPagamento bmConfiguracaoPagamento = new BMConfiguracaoPagamento();

                if (IdConfiguracaoPagamento > 0)
                {
                    configuracaoPagamento = bmConfiguracaoPagamento.ObterPorID(IdConfiguracaoPagamento);
                }

                bmConfiguracaoPagamento.Excluir(configuracaoPagamento);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public ConfiguracaoPagamento ObterConfiguracaoPagamentoPorId(int pId)
        {
            using (BMConfiguracaoPagamento configBM = new BMConfiguracaoPagamento())
            {
                return configBM.ObterPorID(pId);
            }
        }


        //public IList<ViewConfiguracaoPagamentoPublicoAlvo> ObterInformacoesDePagamento(ConfiguracaoPagamentoDTOFiltro filtro)
        //{
        //    return new BMViewConfiguracaoPagamentoPublicoAlvo().ObterInformacoesDePagamento(filtro);
        //}

        #region "Chamada à procedure que obtem informações sobre configuração de pagamento"

        public IList<DTOConfiguracaoPagamentoPublicoAlvo> ObterInformacoesDePagamentoPorFiltro(ConfiguracaoPagamentoDTOFiltro filtro)
        {
            return new ProcConfiguracaoPagamentoPublicoAlvo().BuscarPorFiltro(filtro);
        }

        #endregion


        public IList<ConfiguracaoPagamento> ObterHistoricoDePagamento(int idConfiguracaoPagamento)
        {
            return new BMConfiguracaoPagamento().ObterHistoricoDePagamento(idConfiguracaoPagamento);
        }

        public bool VerificarArquivoDeCobrancaRcb001(string pNomeArquivo, string DataEntrada)
        {
            return new BMConfiguracaoPagamento().VerificarArquivoDeCobrancaEletronicaRcb001(pNomeArquivo, DataEntrada);
        }

        public bool VerificarDataArquivoDeCobrancaRcb001(string DataEntrada)
        {
            return new BMConfiguracaoPagamento().VerificarDataArquivoDeCobrancaEletronicaRcb001(DataEntrada);
        }

        public IList<NivelOcupacional> ObterNivelOcupacionalTodos()
        {
            using (BMNivelOcupacional noBM = new BMNivelOcupacional()) { return noBM.ObterTodos(); }
        }

        public IList<Uf> ObterUFTodos()
        {
            using (BMUf ufBm = new BMUf()) { return ufBm.ObterTodos(); }
        }

        public IList<Perfil> ObterPerfilsTodos()
        {
            using (BMPerfil pfBM = new BMPerfil()) { return pfBM.ObterTodos(); }
        }

        public NivelOcupacional ObterNivelOcupacionalPorID(int pId)
        {
            using (BMNivelOcupacional bm = new BMNivelOcupacional()) { return bm.ObterPorID(pId); }
        }

        public Uf ObterUFPorID(int pId)
        {
            using (BMUf uf = new BMUf()) { return uf.ObterPorId(pId); }
        }

        public Perfil ObterPerfilPorID(int pId)
        {
            using (BMPerfil bm = new BMPerfil()) { return bm.ObterPorId(pId); }
        }

        #endregion


    }
}
