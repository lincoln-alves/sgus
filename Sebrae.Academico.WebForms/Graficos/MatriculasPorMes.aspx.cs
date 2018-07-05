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
    public partial class MatriculasPorMes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strAno = Request.QueryString["ano"];
            string strUF = Request.QueryString["uf"];
            try
            {
                int ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno.ToString()) : DateTime.Now.Year;
                int uf = (!string.IsNullOrEmpty(strUF)) ? int.Parse(strUF.ToString()) : -1;

                var bmUsuario = new BMUsuario();
                if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUfLogadoSeGestor() > 0)
                    uf = bmUsuario.ObterUfLogadoSeGestor();

                GerarGrafico(ano, uf);
            }
            catch
            {
                return;
            }

        }

        public void GerarGrafico(int ano, int uf)
        {
            try
            {
                IList<DTOMatriculasPorMes> Lista = new ManterDashBoard().ObterMatriculasPorMes(ano, uf);

                if (Lista != null && Lista.Count > 0)
                {
                    this.MontarGrafico(this.chartMatriculasMes, Lista, ano);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        private void MontarGrafico(Chart grafico, IList<DTOMatriculasPorMes> dados, int ano)
        {

            grafico.Titles.Add(new Title(string.Format("Matrículas por mês - {0}", ano.ToString())));


            //graficoTaxaAprovacaoNoAno.Titles[0].TextOrientation = TextOrientation.Horizontal;
            //graficoTaxaAprovacaoNoAno.Titles[0].TextStyle = TextStyle.Embed;
            grafico.Series["QuantidadeOnline"].ChartType = SeriesChartType.FastLine;
            grafico.Series["QuantidadeOnline"]["LabelStyle"] = "Top";
            grafico.Series["QuantidadeOnline"]["PointWidth"] = "0.3";
            grafico.Series["QuantidadeOnline"].IsValueShownAsLabel = false;
            grafico.Series["QuantidadeOnline"]["BarLabelStyle"] = "Center";
            grafico.Series["QuantidadeOnline"]["DrawingStyle"] = "Default";
            //grafico.Series["QuantidadeOnline"].IsVisibleInLegend = false;
            grafico.Series["QuantidadeOnline"].LegendText = "Curso on-line";
            grafico.Series["QuantidadeOnline"].LegendToolTip = "Curso on-line";
            //graficoTaxaAprovacaoNoAno.Titles[0].Text = string.Format("Matrículas no Ano de {0} Por Forma de Aquisição", ano.ToString());
            grafico.Series["QuantidadeOnline"].Points.DataBindXY(dados, "Mes", dados, "qtdCursoOnline");
            grafico.Series["QuantidadeOnline"]["ShowMarkerLines"] = "True";



            grafico.Series["QuantidadeCompany"].ChartType = SeriesChartType.FastLine;
            grafico.Series["QuantidadeCompany"]["LabelStyle"] = "Top";
            grafico.Series["QuantidadeCompany"]["PointWidth"] = "0.3";
            grafico.Series["QuantidadeCompany"].IsValueShownAsLabel = false;
            grafico.Series["QuantidadeCompany"]["BarLabelStyle"] = "Center";
            grafico.Series["QuantidadeCompany"]["DrawingStyle"] = "Default";
            //grafico.Series["QuantidadeCompany"].IsVisibleInLegend = false;
            grafico.Series["QuantidadeCompany"].LegendText = "Curso in company";
            grafico.Series["QuantidadeCompany"].LegendToolTip = "Curso in company";
            grafico.Series["QuantidadeCompany"].Points.DataBindXY(dados, "Mes", dados, "qtdCursoInCompany");
            grafico.Series["QuantidadeCompany"]["ShowMarkerLines"] = "True";


            //eixo x (eixo horizontal)
            grafico.ChartAreas["ChartArea1"].AxisX.Title = "Mês";
            grafico.ChartAreas["ChartArea1"].AxisX.TitleAlignment = StringAlignment.Center;
            grafico.ChartAreas["ChartArea1"].AxisX.IsStartedFromZero = false;
            grafico.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            grafico.ChartAreas["ChartArea1"].AxisX.Maximum = 12;
            grafico.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 45;
            grafico.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 1;
            grafico.ChartAreas["ChartArea1"].AxisX.Interval = 1;


            grafico.ChartAreas[0].AxisX.LabelStyle.Font = grafico.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 11, GraphicsUnit.Pixel);

            //eixo y (eixo vertical)
            //grafico.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;

        }

    }
}