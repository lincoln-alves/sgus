using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucDashBoardMatriculasNoAnoPorFormaDeAquisicao : System.Web.UI.UserControl
    {
        public void GerarGraficoDeTotalDeMatriculasNoAnoPorFormaAquisicao()
        {
            try
            {
                //TODO: Nardo -> Tirar este valor fixo.
                //int anoAtual = 2013; // DateTime.Today.Year;
                IList<DTOMatriculaOfertaNoAno> ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao =
                            new ManterDashBoard().ObterTotalMatriculasNoAnoPorFormaAquisicao(null, null);

                if (ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao != null && ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao.Count > 0)
                {
                    this.MontarGraficoDeColunaComTotalDeMatriculasNoAnoPorFormaDeAquisicao(this.chartMatriculasNoAnoPorFormaDeAquisicao,
                                                                                           ListaComTotalDeMatriculasNoAnoPorFormaDeAquisicao, 2013);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        #region "GraficoDeTotalDeMatriculasNoAnoPorFormaAquisicao"
        
        private void MontarGraficoDeColunaComTotalDeMatriculasNoAnoPorFormaDeAquisicao(Chart graficoTaxaAprovacaoNoAno, IList<DTOMatriculaOfertaNoAno> dados, int ano)
        {

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
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;

            //eixo y (eixo vertical)
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisY.Title = "Quantidade de Matrículas";
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;

        }

        #endregion
    }
}