using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class ConcluintesPorTipoSolucao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strAno = Request.QueryString["ano"];
            string strUF = Request.QueryString["uf"];
            try
            {
                int ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno.ToString()) : DateTime.Now.Year;
                int uf = (!string.IsNullOrEmpty(strUF)) ? int.Parse(strUF.ToString()) : -1;

                //var bmUsuario = new BMUsuario();
                //if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUFLogadoSeGestor() > 0)
                //    uf = bmUsuario.ObterUFLogadoSeGestor();

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
                IList<DTOConcluintesPorFormaAquisicao> Lista = new ManterDashBoard().ObterConcluintesPorFormaAquisicao(ano, uf);

                if (Lista != null && Lista.Count > 0)
                {
                    this.MontarGrafico(this.chartMatriculasPorFormaAquisicao, Lista, ano);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        private void MontarGrafico(Chart grafico, IList<DTOConcluintesPorFormaAquisicao> dados, int ano)
        {
            grafico.Titles.Add(new Title(string.Format("Ano: {0}", ano.ToString()), Docking.Top, new Font("Arial", 16, GraphicsUnit.Pixel), Color.Black));

            Series serie1 = new Series();
            serie1.ChartType = SeriesChartType.Column;
            serie1.IsValueShownAsLabel = true;
            serie1.Color = Color.FromArgb(135, 206, 235);
            serie1["PointWidth"] = "0.3";
            serie1.Name = "FormaAquisicao";
            serie1.Points.DataBindXY(dados, "FormaAquisicao", dados, "Percentual");
            serie1["ShowMarkerLines"] = "false";
            grafico.Series.Add(serie1);


            ChartArea area = new ChartArea();


            //eixo x (eixo horizontal)
            area.Name = "ChatArea2";
            //area.AxisX.Title = "Forma Aquisicao";
            area.AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            area.AxisX.IsStartedFromZero = false;
            area.AxisX.IntervalOffset = 1;
            area.AxisX.Interval = 1;
            area.AxisX.LineColor = Color.Black;
            area.AxisX.LabelStyle.Font = new Font("Arial", 9, GraphicsUnit.Pixel);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);


            area.AxisY.IntervalOffset = 5;
            area.AxisY.Interval = 5;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);
            area.AxisY.LineColor = Color.Black;
            area.AxisY.LabelStyle.Font = new Font("Arial", 6, GraphicsUnit.Pixel);

            grafico.ChartAreas.Add(area);


            //eixo y (eixo vertical)
            //grafico.ChartAreas["ChartArea1"].AxisY.TextOrientation = TextOrientation.Rotated270;

        }
    }
}