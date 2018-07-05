using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using System.Linq;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Tela de Dashboard.
    /// </summary>
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "dynamic-form", "/js/dynamic-form.js");
            PreencherListaUf();

            if (!Page.IsPostBack)
            {
                this.CarregarInformacoesDasTabelas();
            }

            if (!new BMUsuario().PerfilAdministrador())
            {
                ucNotificacoes1.Visible =
                ucDashBoardRelatoriosMaisAcessados1.Visible =
                ucDashboardFuncionalidadesMaisAcessadas1.Visible = false;
            }
        }

        private void CarregarInformacoesDasTabelas()
        {
            try
            {
                //ucDashBoardRelatoriosMaisAcessados1.CarregarInformacoesSobreRelatoriosMaisAcessados();
                this.GerarTabelas();
                this.GerarGraficos();
            }
            catch
            {
                //TODO: -> Tratar Exceção
                //throw ex;
            }
        }

        private void GerarTabelas()
        {
            this.ucDashBoardRelatoriosMaisAcessados1.CarregarInformacoesSobreRelatoriosMaisAcessados();
            this.ucDashboardFuncionalidadesMaisAcessadas1.CarregarInformacoesSobreFuncionalidadesMaisAcessadas();
            this.ucNotificacoes1.CarregarInformacoesSobreNotificacoes();
        }

        private void GerarGraficos()
        {
            //Todo: Descomentar para ver o retorno do Parâmetro de output
            //this.GerarGraficoDeTaxaDeAprovacaoNoAno();

            //this.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios1.ger
            //this.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios1.
            //this.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios1 .GerarGraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios();
        }

        public void GerarGraficoDeTaxaDeAprovacaoNoAno()
        {
            try
            {
                int anoAtual = DateTime.Today.Year;
                IList<DTOMatriculaOferta> ListaDadosComTaxaDeAprovacaoNoAno = new ManterDashBoard().ObterTaxaDeAprovacaoNoAno(anoAtual, null);

                if (ListaDadosComTaxaDeAprovacaoNoAno != null && ListaDadosComTaxaDeAprovacaoNoAno.Count > 0)
                {
                    //WebFormHelper.PreencherGrid(ListaDadosComTaxaDeAprovacaoNoAno, dgvInformacoesDeRelatoriosMaisAcessados);
                    //this.MontarGrafico(this.chartTaxaAprovacaoNoAno, SeriesChartType.Bar, ListaDadosComTaxaDeAprovacaoNoAno);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void PreencherListaUf()
        {
            WebFormHelper.PreencherLista(new ManterUf().ObterTodosUf(), ddlUF, false, true);
        }

        #region "GraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios"



        #endregion

        #region "GraficoDeTotalDeMatriculasNoAnoPorUf"


        #endregion


        //private void MontarGrafico(Chart graficoTaxaAprovacaoNoAno, SeriesChartType chartType, IList<DTOMatriculaOferta> dados)
        //{
        //    graficoTaxaAprovacaoNoAno.DataSource = dados;
        //    graficoTaxaAprovacaoNoAno.DataBind();
        //}



    }
}