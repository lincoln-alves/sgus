using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class InscritosPorCategoria : System.Web.UI.Page
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
                IList<DTOInscritosPorCategoria> Lista = new ManterDashBoard().ObterInscritosPorCategoria(ano, uf);

                if (Lista != null && Lista.Count > 0)
                {
                    this.MontarGrafico(this.chartInscritosPorCategoria, Lista, ano);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        private void MontarGrafico(Chart grafico, IList<DTOInscritosPorCategoria> dados, int ano)
        {

            grafico.Titles.Add(new Title(string.Format("Ano: {0}", ano.ToString()), Docking.Top, new Font("Arial", 16, GraphicsUnit.Pixel), Color.Black));
                

            Series serie1 = new Series();
            serie1.ChartType = SeriesChartType.Pie;
            serie1.IsValueShownAsLabel = true;
            serie1.Name = "FormaAquisicao";
            serie1.Points.DataBindXY(dados, "Categoria", dados, "QuantidadeInscritos");
            //serie1.ToolTip = "Tooltip";
            serie1["PieLabelStyle"] = "Outside";
            grafico.Series.Add(serie1);


            Legend legenda = new Legend();
            legenda.Name = "Legenda";

            //legenda.HeaderSeparator = LegendSeparatorStyle.Line;
            //legenda.BackSecondaryColor = Color.FromArgb(57, 116, 172);


            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Cor";
            firstColumn.HeaderBackColor = Color.FromArgb(57, 116, 172);
            firstColumn.HeaderForeColor = Color.White;
            legenda.CellColumns.Add(firstColumn);

            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Categoria";
            //secondColumn.HeaderAlignment = StringAlignment.;
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.Alignment = ContentAlignment.MiddleLeft;
            secondColumn.HeaderBackColor = Color.FromArgb(57, 116, 172);
            secondColumn.HeaderForeColor = Color.White;
            legenda.CellColumns.Add(secondColumn);

            // Add Total cell column      
            LegendCellColumn percColumn = new LegendCellColumn();
            percColumn.Text = "#PERCENT";
            percColumn.HeaderText = "%";
            percColumn.Name = "PercColumn";
            percColumn.HeaderBackColor = Color.FromArgb(57, 116, 172);
            percColumn.HeaderForeColor = Color.White;
            legenda.CellColumns.Add(percColumn);

            grafico.Legends.Add(legenda);

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


            area.Area3DStyle.IsClustered = true;
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Perspective = 10;
            area.Area3DStyle.PointGapDepth = 900;
            area.Area3DStyle.IsRightAngleAxes = false;
            area.Area3DStyle.WallWidth = 25;
            //area.Area3DStyle.Rotation = 65;
            area.Area3DStyle.Inclination = 50;


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