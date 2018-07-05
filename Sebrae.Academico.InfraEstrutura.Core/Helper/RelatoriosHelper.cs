using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Web;
using System.Collections;
using System.Reflection;
using System.IO;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.InfraEstrutura.Core.Helper
{
    public class RelatoriosHelper
    {
        public static ReportViewer GerarReportViewerHistoricoAcademico(string caminhoRDLC,
                                                                       IEnumerable DadosGerais, IEnumerable lstCursos,
                                                                       IEnumerable lstTrilha, IEnumerable lstPrograma,
                                                                       IEnumerable lstExtracurricular)
        {

            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            Assembly assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoRDLC);

            ReportViewer rv = new ReportViewer();
            rv.LocalReport.LoadReportDefinition(stream);
            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DadosGerais));
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", lstCursos));
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", lstTrilha));
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet4", lstPrograma));
            rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet5", lstExtracurricular));
            rv.LocalReport.Refresh();

            return rv;

        }

        public static void GerarReportViewerHistoricoAcademicoPDF(string caminhoRDLC,
                                                                       IEnumerable DadosGerais, IEnumerable lstCursos,
                                                                       IEnumerable lstTrilha, IEnumerable lstPrograma,
                                                                       IEnumerable lstExtracurricular)
        {

            var rv = GerarReportViewerHistoricoAcademico(caminhoRDLC, DadosGerais, lstCursos, lstTrilha, lstPrograma, lstExtracurricular);

            // Mostra na tela o PDF
            byte[] arradeBytes = rv.LocalReport.Render("PDF");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachement; filename=\"{0}\"", "HistoricoAcademico.pdf"));
            HttpContext.Current.Response.AddHeader("Content-Length", arradeBytes.Length.ToString());
            HttpContext.Current.Response.OutputStream.Write(arradeBytes, 0, arradeBytes.Length);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

        }

        public static byte[] GerarReportViewerHistoricoAcademicoBytes(string caminhoRDLC,
                                                                       IEnumerable DadosGerais, IEnumerable lstCursos,
                                                                       IEnumerable lstTrilha, IEnumerable lstPrograma,
                                                                       IEnumerable lstExtracurricular)
        {
            var rv = GerarReportViewerHistoricoAcademico(caminhoRDLC, DadosGerais, lstCursos, lstTrilha, lstPrograma, lstExtracurricular);

            // Mostra na tela o PDF
            byte[] arradeBytes = rv.LocalReport.Render("PDF");
            return arradeBytes;
        }
    }
}
