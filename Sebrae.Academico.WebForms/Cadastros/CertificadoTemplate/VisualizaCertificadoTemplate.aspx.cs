using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.WebForms.Cadastros.CertificadoTemplate
{
    public partial class VisualizaCertificadoTemplate : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Id"] != null)
            {
                var idCertificadoTemplate = int.Parse(Request["Id"]);
                var manterCertificadoTemplate = new ManterCertificadoTemplate();
                var certificadoTemplate = manterCertificadoTemplate.ObterCertificadoTemplatePorID(idCertificadoTemplate);
                var dt = GerarDataTableComCertificado(certificadoTemplate);
                
                var bt = RetornarCertificado(certificadoTemplate, dt);
                Response.AddHeader("Content-Disposition", "Inline; filename=certificado_" + idCertificadoTemplate + ".pdf");
                Response.ContentType = "Application/pdf";
                Response.ContentEncoding = Encoding.UTF8;
                Response.BinaryWrite(bt);
                Response.Flush();
            }
        }

        public byte[] RetornarCertificado(Dominio.Classes.CertificadoTemplate cf, DataTable dt)
        {
            string caminhoReport;

            if (!string.IsNullOrWhiteSpace(cf.TextoCertificado2) && !string.IsNullOrEmpty(cf.Imagem2))
            {
                caminhoReport = "EmitirCertificado.rptCertificadoTemplate2paginas.rdlc";
            }
            else
            {
                caminhoReport = "EmitirCertificado.rptCertificadoTemplate.rdlc";
            }

            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
            var stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);

            var rv = new ReportViewer();
            rv.LocalReport.LoadReportDefinition(stream);
            rv.LocalReport.DataSources.Clear();
            rv.LocalReport.DataSources.Add(new ReportDataSource("dsCertificadoTemplate", dt));
            return rv.LocalReport.Render("PDF");
        }

        public DataTable GerarDataTableComCertificado(Dominio.Classes.CertificadoTemplate cf)
        {
            if (cf == null)
                throw new Exception("Não existem certificados disponíveis para emissão.");

            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("TX_Certificado"));
            dt.Columns.Add(new DataColumn("OB_Imagem", typeof(byte[])));

            var dr = dt.NewRow();

            dr["TX_Certificado"] = FormatarTextoCertificadoTurma(cf.TextoDoCertificado);
            dr["OB_Imagem"] = ObterImagemBase64(cf.Imagem);

            if (!string.IsNullOrWhiteSpace(cf.TextoCertificado2) && !string.IsNullOrEmpty(cf.Imagem2))
            {
                dt.Columns.Add(new DataColumn("TX_Certificado2"));
                dt.Columns.Add(new DataColumn("OB_Image2", typeof(byte[])));
                dr["TX_Certificado2"] = FormatarTextoCertificadoTurma(cf.TextoCertificado2);
                dr["OB_Image2"] = ObterImagemBase64(cf.Imagem2);
            }

            dt.Rows.Add(dr);

            return dt;
        }

        private object FormatarTextoCertificadoTurma(string textoDoCertificado)
        {
            return "*Dados fictícios\n\n" +
                    textoDoCertificado
                      .Replace("#DATASISTEMA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATASISTEMAEXTENSO", DateTime.Now.ToLongDateString().ToString())
                      .Replace("#DATAHORASISTEMA", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                      .Replace("#DATAGERACAOCERTIFICADO", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#ALUNO", "José da Silva")
                      .Replace("#CPF", "111.111.111.11")
                      .Replace("#EMAILALUNO", "jose@gmail.com")
                      .Replace("#OFERTA", "Curso de WebAula")
                      .Replace("#NOMEPROGRAMA", "Programa de capacitação")
                      .Replace("#DATAINICIOOFERTA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATAFIMOFERTA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATAMATRICULA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATATERMINO", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#TURMA", "Turma A")
                      .Replace("#CODIGOCERTIFICADO", "00000000000000000000000000000000")
                      .Replace("#DATEGERACAOCERTIFICADO", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#STATUS", "Inscrito")
                      .Replace("#DATAINICIOTURMA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATAFIMTURMA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#CARGAHORARIA", "1")
                      .Replace("#NOMESE", "SE")
                      .Replace("#EMENTA", "Topicos Auxiliares")
                      .Replace("#PROFESSOR", "João")
                      .Replace("#MEDIAFINALTURMA", "10")
                      .Replace("#NOTAFINAL", "10")
                      .Replace("#TRILHANIVEL", "Nível")
                      .Replace("#TRILHA", "Trilha")
                      .Replace("#DATALIMITE", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATAINICIOTRILHA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#DATAFIMTRILHA", DateTime.Now.ToString("dd/MM/yyyy"))
                      .Replace("#MEDIAFINALTRILHA", "10")
                      .Replace("#LOCAL", "Brasília, DF")
                      .Replace("#TEXTOPORTAL", "Texto do portal")
                      .Replace("#INFORMACOESADICIONAIS", "Informações adicionais do certificado")
                      .Replace("#AREATEMATICA", "Área temática do certificado")
                      .Replace("#CARGAHORARIASOLUCAOSEBRE", "10");
        }

        public static byte[] ObterImagemBase64(string pImagem)
        {
            string str64 = pImagem.Substring(pImagem.IndexOf(",") + 1);
            return Convert.FromBase64String(str64);
        }
    }
}