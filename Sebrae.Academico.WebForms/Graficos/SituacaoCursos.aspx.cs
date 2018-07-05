using System;
using System.Collections.Generic;
using System.Web.UI;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Microsoft.Reporting.WebForms;
using System.Reflection;
using System.IO;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class SituacaoCursos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GerarRelatorio();
            }
        }

        private void GerarRelatorio()
        {
            RelatorioSituacaoCursos relatorio = new RelatorioSituacaoCursos();
            
            string strAno = Request.QueryString["ano"];
            int ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno.ToString()) : DateTime.Now.Year;

            IList<DTOSituacaoCursos> ListaComInformacoes = relatorio.ConsultarSituacoes(null, ano);
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            Assembly assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + "SituacaoCursos.rptTabelaSituacaoCursos.rdlc");

            rpt1.LocalReport.LoadReportDefinition(stream);
            rpt1.LocalReport.DataSources.Clear();
            rpt1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ListaComInformacoes));

            rpt1.ProcessingMode = ProcessingMode.Local;

            
        }


    }
}