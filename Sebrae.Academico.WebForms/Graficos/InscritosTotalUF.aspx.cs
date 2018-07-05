using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BM.Classes;
using System.Drawing;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class InscritosTotalUF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strAno = Request.QueryString["ano"];
            string strUF = Request.QueryString["uf"];
            try
            {
                int ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno.ToString()) : DateTime.Now.Year;

                //var bmUsuario = new BMUsuario();
                //if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUFLogadoSeGestor() > 0)
                //    uf = bmUsuario.ObterUFLogadoSeGestor();

                GerarGrafico(ano);
            }
            catch
            {
                return;
            }
        }
        public void GerarGrafico(int ano)
        {
            try
            {
                IList<DTOInscritosTotalUF> Lista = new ManterDashBoard().ObterInscritosTotalUF(ano);

                if (Lista != null && Lista.Count > 0)
                {
                    this.MontarGrafico(this.chartInscritosTotalUF, Lista, ano);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        private void MontarGrafico(Chart grafico, IList<DTOInscritosTotalUF> dados, int ano)
        {

            grafico.Titles.Add(new Title(string.Format("Ano: {0}", ano.ToString()), Docking.Top, new Font("Arial", 16, GraphicsUnit.Pixel), Color.Black));


            Series serie1 = new Series();
            serie1.ChartType = SeriesChartType.Bar;
            serie1.IsValueShownAsLabel = true;
            serie1.Color = Color.FromArgb(135, 206, 235);
            serie1["PointWidth"] = "0.7";
            serie1.Name = "Inscritos";
            serie1.Points.DataBindXY(dados, "UF", dados, "QuantidadeInscritos");
            serie1["ShowMarkerLines"] = "false";
            grafico.Series.Add(serie1);

            Series serie2 = new Series();
            serie2.ChartType = SeriesChartType.Bar;
            serie2.IsValueShownAsLabel = true;
            serie2.Color = Color.FromArgb(50, 205, 50);
            serie2["PointWidth"] = "0.7";
            serie2.Name = "Total";
            serie2.Points.DataBindXY(dados, "UF", dados, "QuantidadeTotal");
            serie2["ShowMarkerLines"] = "false";
            grafico.Series.Add(serie2);

            ChartArea area = new ChartArea();

            Legend legenda = new Legend();
            legenda.Name = "Legenda";
            legenda.Position.Auto = false;
            legenda.Position = new ElementPosition(5, 95, 100, 5);
            legenda.IsTextAutoFit = true;
            grafico.Legends.Add(legenda);

            //eixo x (eixo horizontal)
            area.Name = "ChatArea2";
            //area.AxisX.Title = "Forma Aquisicao";
            area.AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            area.AxisX.IsStartedFromZero = false;
            area.AxisX.IntervalOffset = 1;
            area.AxisX.Interval = 1;
            area.AxisX.LineColor = Color.Black;
            area.AxisX.LabelStyle.Font = new Font("Arial", 1, GraphicsUnit.Pixel);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);


            //area.AxisY.IntervalOffset = 5;
            //area.AxisY.Interval = 5;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);
            area.AxisY.LineColor = Color.Black;
            area.AxisY.LabelStyle.Font = new Font("Arial", 1, GraphicsUnit.Pixel);

            grafico.ChartAreas.Add(area);


            //eixo y (eixo vertical)
            //grafico.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;

        }
    }
}