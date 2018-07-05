using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios : System.Web.UI.UserControl
    {
        public void GerarGraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios()
        {
            try
            {
                int? anoAtual = 2013;
                IList<DTOEstatisticaNivelOcupacional> ListaComParticipacaoProporcionalAoNumeroDeFuncionarios =
                                        new ManterDashBoard().ObterParticipacaoProporcionalAoNumeroDeFuncionarios(anoAtual, null);

                if (ListaComParticipacaoProporcionalAoNumeroDeFuncionarios != null && ListaComParticipacaoProporcionalAoNumeroDeFuncionarios.Count > 0)
                {
                    this.MontarGraficoDeColunaComParticipacaoProporcionalAoNumeroDeFuncionarios(this.chartParticipacaoProporcionalAoNumeroDeFuncionarios, ListaComParticipacaoProporcionalAoNumeroDeFuncionarios, anoAtual);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void MontarGraficoDeColunaComParticipacaoProporcionalAoNumeroDeFuncionarios(Chart chartParticipacaoProporcionalAoNumeroDeFuncionarios,
                                        IList<DTOEstatisticaNivelOcupacional> dados, int? ano)
        {

            
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.DataSource = dados;
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series[0].YValueMembers = "QuantidadeAtivos";
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series[1].YValueMembers = "QuantidadeAtivosComInscricao";
            //'Set the X-axle as date value  
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series[0].XValueMember = "NomeNivelOcupacional";
            //Bind the Chart control with the setting above 
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas[0].AxisX.Interval = 1;
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas[0].AxisX.LabelStyle.Angle = -90; //.Series[0].LabelAngle = 90;
            chartParticipacaoProporcionalAoNumeroDeFuncionarios.DataBind();

            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Vendor A"].Points.DataBindXY(dados, "NomeNivelOcupacional", dados, "QuantidadeAtivos");
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Vendor B"].Points.DataBindXY(dados, "NomeNivelOcupacional", dados, "QuantidadeAtivosComInscricao");

            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.ApplyPaletteColors();  

            //for (int i = 0; i < dados.Count; i++)
            //{
            //    Series series = new Series();
            //    int y = dados[i].QuantidadeAtivos;
            //    series.Points.AddXY(dados[i].NomeNivelOcupacional, y);

            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series.Add(series);
            //}




            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Titles.Add(new Title("Titulo"));
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Titles[0].TextOrientation = TextOrientation.Horizontal;
            ////graficoTaxaAprovacaoNoAno.Titles[0].TextStyle = TextStyle.Embed;

            ////graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].ChartType = SeriesChartType.Column;
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"]["LabelStyle"] = "Top";
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"]["PointWidth"] = "0.3";
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].IsValueShownAsLabel = true;
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"]["BarLabelStyle"] = "Center";
            ////graficoTaxaAprovacaoNoAno.Series["Ano"]["DrawingStyle"] = "Cylinder";
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].IsVisibleInLegend = true;

            //if (ano.HasValue)
            //{
            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].LegendText = string.Format("Ano de {0}", ano.ToString());
            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].LegendToolTip = string.Format("Total de Matrículas no Ano de {0}", ano.ToString());
            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Titles[0].Text = string.Format("Matrículas no Ano de {0} Por UF", ano.ToString());
            //}
            //else
            //{
            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].LegendText = "Todos os Anos";
            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].LegendToolTip = "Total de Matrículas de todos os Anos";
            //    chartParticipacaoProporcionalAoNumeroDeFuncionarios.Titles[0].Text = "Matrículas de todos os anos Por UF";
            //}

            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["Ativos"].Points.DataBindXY(dados, "NomeNivelOcupacional", dados, "QuantidadeAtivos"); //, dados, "AtivosComInscricao");

            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.Series["AtivosComInscricao"].Points.DataBindXY(dados, "NomeNivelOcupacional", dados, "QuantidadeAtivosComInscricao"); //, dados, "AtivosComInscricao");

            ////graficoTaxaAprovacaoNoAno.Series["Ano"]["ShowMarkerLines"] = "True";

            ////eixo x (eixo horizontal)
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas["ChartArea1"].AxisX.Title = "N[ivel Ocupacional";
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas["ChartArea1"].AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;

            ////eixo y (eixo vertical)
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas["ChartArea1"].AxisY.Title = "Quantidade de Matrículas";
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas["ChartArea1"].AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            //chartParticipacaoProporcionalAoNumeroDeFuncionarios.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;


        }

    }
}