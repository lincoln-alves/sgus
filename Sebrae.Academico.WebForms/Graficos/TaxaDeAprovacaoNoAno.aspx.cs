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
    public partial class TaxaDeAprovacaoNoAno : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GerarGraficoDeTotalDeMatriculasNoAnoPorUf();
        }

        public void GerarGraficoDeTotalDeMatriculasNoAnoPorUf()
        {
            try
            {
                string strAno = Request.QueryString["ano"];
                string strUF = Request.QueryString["ddlUF"];

                int ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno.ToString()) : DateTime.Now.Year;
                int uf = (!string.IsNullOrEmpty(strUF)) ? int.Parse(strUF.ToString()) : -1;

                var bmUsuario = new BMUsuario();
                if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUfLogadoSeGestor() > 0)
                    uf = bmUsuario.ObterUfLogadoSeGestor();

                IList<DTOTotalMatriculaOfertaPorAno> ListaComTotalDeMatriculasNoAnoPorUf =
                                            new ManterDashBoard().ObterTotalMatriculasNoAnoPorUf(ano, uf);

                if (ListaComTotalDeMatriculasNoAnoPorUf != null && ListaComTotalDeMatriculasNoAnoPorUf.Count > 0)
                {
                    this.MontarGraficoDeColunaComTotalDeMatriculasNoAnoPorUf(this.chartMatriculasNoAnoPorUf, ListaComTotalDeMatriculasNoAnoPorUf, ano);
                }

            }
            catch
            {
                //throw ex;
            }
        }

        private void MontarGraficoDeColunaComTotalDeMatriculasNoAnoPorUf(Chart graficoTotalDeMatriculasNoAnoPorUf, IList<DTOTotalMatriculaOfertaPorAno> dados, int? ano)
        {
            //graficoTotalDeMatriculasNoAnoPorUf.Titles.Add(new Title("Titulo"));
            //graficoTotalDeMatriculasNoAnoPorUf.Titles[0].TextOrientation = TextOrientation.Horizontal;
            //graficoTaxaAprovacaoNoAno.Titles[0].TextStyle = TextStyle.Embed;

            //graficoTaxaAprovacaoNoAno.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].ChartType = SeriesChartType.Column;
            graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"]["PointWidth"] = "0.75";
            //graficoTaxaAprovacaoNoAno.Series["Ano"]["DrawingStyle"] = "Cylinder";
            graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].IsVisibleInLegend = true;

            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

            if (ano.HasValue)
            {
                graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].LegendText = string.Format("Ano de {0}", ano.ToString());
                graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].LegendToolTip = string.Format("Total de Matrículas no Ano de {0}", ano.ToString());
                //graficoTotalDeMatriculasNoAnoPorUf.Titles[0].Text = string.Format("Matrículas no Ano de {0} Por UF", ano.ToString());
            }
            else
            {
                graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].LegendText = "Todos os Anos";
                graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].LegendToolTip = "Total de Matrículas de todos os Anos";
                //graficoTotalDeMatriculasNoAnoPorUf.Titles[0].Text = "Matrículas de todos os anos Por UF";
            }

            graficoTotalDeMatriculasNoAnoPorUf.Series["Ano"].Points.DataBindXY(dados, "SiglaUf", dados, "Quantidade");
            //graficoTaxaAprovacaoNoAno.Series["Ano"]["ShowMarkerLines"] = "True";

            //eixo x (eixo horizontal)
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisX.Title = "UF";
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisX.TitleAlignment = StringAlignment.Center;
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;

            //eixo y (eixo vertical)
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisY.Title = "Quantidade de Matrículas";
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisY.TitleAlignment = StringAlignment.Center;
            graficoTotalDeMatriculasNoAnoPorUf.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;

            
        }
    }
}