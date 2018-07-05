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
    public partial class CursosMaiorNumeroConcluintes : System.Web.UI.Page
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
            RelatorioCursosMaiorNumeroConcluintes relatorio = new RelatorioCursosMaiorNumeroConcluintes();

            string strAno = Request.QueryString["ano"];
            int ano = (!string.IsNullOrEmpty(strAno)) ? int.Parse(strAno.ToString()) : DateTime.Now.Year;

            IList<DTOCursosMaiorNumeroConcluintes> ListaComInformacoes = relatorio.ConsultarConcluintes(null, ano);
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            Assembly assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + "CursosMaiorNumeroConcluintes.rptCursosMaiorNumeroConcluintes.rdlc");

            rpt1.LocalReport.LoadReportDefinition(stream);
            rpt1.LocalReport.DataSources.Clear();
            rpt1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ListaComInformacoes));

            rpt1.ProcessingMode = ProcessingMode.Local;


        }
    }
}