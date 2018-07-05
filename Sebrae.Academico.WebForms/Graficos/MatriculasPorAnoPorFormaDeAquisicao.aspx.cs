using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class MatriculasPorAnoPorFormaDeAquisicao : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strAno = Request.QueryString["ano"];
            string strUF = Request.QueryString["uf"];
            try
            {
                int ano = (!string.IsNullOrEmpty(strAno))?int.Parse(strAno.ToString()):DateTime.Now.Year;
                int uf = (!string.IsNullOrEmpty(strUF))?int.Parse(strUF.ToString()):-1;

                var bmUsuario = new BMUsuario();
                if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUfLogadoSeGestor() > 0)
                    uf = bmUsuario.ObterUfLogadoSeGestor();

                GerarGraficoDeTotalDeMatriculasNoAnoPorFormaAquisicao(ano, uf);
            }
            catch
            {
                return;
            }
            
        }

        public void GerarGraficoDeTotalDeMatriculasNoAnoPorFormaAquisicao(int ano, int uf)
        {
            try
            {
                IList<DTOMatriculaOfertaNoAno> ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao =
                            new ManterDashBoard().ObterTotalMatriculasNoAnoPorFormaAquisicao(ano, uf);

                if (ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao != null && ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao.Count > 0)
                {
                    this.MontarGraficoDeColunaComTotalDeMatriculasNoAnoPorFormaDeAquisicao(this.chartMatriculasNoAnoPorFormaDeAquisicao,
                                                                                           ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao, ano);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        private void MontarGraficoDeColunaComTotalDeMatriculasNoAnoPorFormaDeAquisicao(Chart graficoTaxaAprovacaoNoAno, IList<DTOMatriculaOfertaNoAno> dados, int ano)
        {

            graficoTaxaAprovacaoNoAno.Titles.Add(new Title(string.Format("Ano: {0}", ano.ToString()), Docking.Top, new Font("Arial", 16, GraphicsUnit.Pixel), Color.Black));

            //graficoTaxaAprovacaoNoAno.Titles.Add(new Title("Titulo"));
            //graficoTaxaAprovacaoNoAno.Titles[0].TextOrientation = TextOrientation.Horizontal;
            //graficoTaxaAprovacaoNoAno.Titles[0].TextStyle = TextStyle.Embed;
            graficoTaxaAprovacaoNoAno.Series["Quantidade"].ChartType = SeriesChartType.Column;
            graficoTaxaAprovacaoNoAno.Series["Quantidade"]["LabelStyle"] = "Top";
            graficoTaxaAprovacaoNoAno.Series["Quantidade"]["PointWidth"] = "0.3";
            graficoTaxaAprovacaoNoAno.Series["Quantidade"].IsValueShownAsLabel = false;
            graficoTaxaAprovacaoNoAno.Series["Quantidade"]["BarLabelStyle"] = "Center";
            graficoTaxaAprovacaoNoAno.Series["Quantidade"]["DrawingStyle"] = "Default";
            graficoTaxaAprovacaoNoAno.Series["Quantidade"].IsVisibleInLegend = false;

            //graficoTaxaAprovacaoNoAno.Series["Ano"].LegendText = ano.ToString();
            graficoTaxaAprovacaoNoAno.Series["Quantidade"].LegendToolTip = string.Format("Total da Forma de Aquisição do Ano de {0}", ano.ToString());
            //graficoTaxaAprovacaoNoAno.Titles[0].Text = string.Format("Matrículas no Ano de {0} Por Forma de Aquisição", ano.ToString());

            graficoTaxaAprovacaoNoAno.Series["Quantidade"].Points.DataBindXY(dados, "NomeFormaAquisicao", dados, "QuantidadeMatriculados");
            graficoTaxaAprovacaoNoAno.Series["Quantidade"]["ShowMarkerLines"] = "True";

            //eixo x (eixo horizontal)
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisX.Title = "Formas de Aquisição";
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisX.TitleAlignment = StringAlignment.Center;

            //eixo y (eixo vertical)
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisY.Title = "Quantidade de Matrículas";
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisY.TitleAlignment = StringAlignment.Center;
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;

        }
    }
}