﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class TurmasPorStatus : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            GerarGraficoTurmasPorStatus();
        }

        public void GerarGraficoTurmasPorStatus()
        {
            try
            {
                var strAno = Request.QueryString["ano"];

                var dataInicio = new DateTime((!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno) : DateTime.Now.Year, 1, 1);

                var dataFim = new DateTime((!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno) : DateTime.Now.Year, 12, 31);

                var manterRelatorio = new RelatorioMonitoramentoTurmas();

                var listaTurmasPorStatus = manterRelatorio.ObterTotalStatus(dataInicio: dataInicio, dataFim: dataFim).Where(x => x.Status != null).ToList();
                
                manterRelatorio.InserirStatusSemDados(listaTurmasPorStatus);

                listaTurmasPorStatus = listaTurmasPorStatus.OrderBy(x => x.Status).ToList();

                if (listaTurmasPorStatus.Any())
                {
                    MontarGraficoDeColuna(chartTurmasPorStatus, listaTurmasPorStatus);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void MontarGraficoDeColuna(Chart chart, IList<DTOMonitoramentoTurma> dados)
        {


            chart.DataSource = dados;
            chart.Series[0].YValueMembers = "TotalTurmasComStatus";
            chart.Series[1].YValueMembers = "TotalTurmasAno";
            chart.Series[0].XValueMember = "StatusFormatado";

            //Bind the Chart control with the setting above 
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            chart.DataBind();
        }
    }
}