using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class ParticipacaoProporcionalAoNumeroDeFuncionarios : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            GerarGraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios();
        }

        public void GerarGraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios()
        {
            try
            {

                var strAno = Request.QueryString["ano"];
                var strUf = Request.QueryString["ddlUF"];

                var ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno) : DateTime.Now.Year;
                var uf = (!string.IsNullOrEmpty(strUf)) ? int.Parse(strUf) : -1;

                var bmUsuario = new BMUsuario();
                if (!bmUsuario.PerfilAdministrador() && bmUsuario.ObterUfLogadoSeGestor() > 0)
                    uf = bmUsuario.ObterUfLogadoSeGestor();

                var listaComParticipacaoProporcionalAoNumeroDeFuncionarios =
                    new ManterDashBoard().ObterParticipacaoProporcionalAoNumeroDeFuncionarios(ano, uf);

                if (listaComParticipacaoProporcionalAoNumeroDeFuncionarios != null && listaComParticipacaoProporcionalAoNumeroDeFuncionarios.Count > 0)
                {
                    MontarGraficoDeColunaComParticipacaoProporcionalAoNumeroDeFuncionarios(this.chartParticipacaoProporcionalAoNumeroDeFuncionarios, listaComParticipacaoProporcionalAoNumeroDeFuncionarios);
                }

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void MontarGraficoDeColunaComParticipacaoProporcionalAoNumeroDeFuncionarios(Chart chart, IList<DTOEstatisticaNivelOcupacional> dados)
        {


            chart.DataSource = dados;
            chart.Series[0].YValueMembers = "QuantidadeAtivos";
            chart.Series[1].YValueMembers = "QuantidadeAtivosComInscricao";
            //'Set the X-axle as date value  
            chart.Series[0].XValueMember = "NomeNivelOcupacional";
            //Bind the Chart control with the setting above 
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90; //.Series[0].LabelAngle = 90;
            chart.DataBind();
        }
    }
}