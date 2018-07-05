using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.BP.DTO.Services.HistoricoAcademico;
//using Sebrae.Academico.Services;
using Sebrae.Academico.Util.Classes;
using System.Data;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Text;
using System.Net;
using System.Web;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User Contrlol de cursos
    /// </summary>
    public partial class ucCursos : System.Web.UI.UserControl
    {
        public Usuario usuario { get; set; }
        public IList<MatriculaOferta> ListaMatriculaOferta { get; set; }
        public IList<DTOItemHistoricoAcademico> ListaHistoricoAcademico { get; set; }

        public void PrepararTelaParaExibirInformacoesDosCursos(int IdUsuario)
        {
            if (IdUsuario > 0)
            {
                this.IdUsuario = IdUsuario;
                this.ListaHistoricoAcademico = new HistoricoAcademicoServices().ConsultarHistorico(IdUsuario);
                this.PreencherGrid();
            }

        }

        private void PreencherGrid()
        {
            var manterMatriculaOferta = new ManterMatriculaOferta();
            ListaMatriculaOferta = manterMatriculaOferta.ObterPorUsuario(IdUsuario).ToList();
            WebFormHelper.PreencherGrid(ListaMatriculaOferta, dgvMatriculaOferta);
        }

        public int IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }

        protected void dgvOferta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                MatriculaOferta matriculaOferta = (MatriculaOferta)e.Row.DataItem;

                var certificado = this.ListaHistoricoAcademico.FirstOrDefault(x => x.IdMatricula == matriculaOferta.ID);

                LinkButton lbEmitirCertificado = (LinkButton)e.Row.FindControl("lbEmitirCertificado");
                lbEmitirCertificado.Visible = false;

                if (certificado != null)
                {
                    if (certificado.TemCertificado)
                    {
                        lbEmitirCertificado.Visible = true;
                    }
                }

                if (matriculaOferta != null)
                {
                    //this.usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);
                    //this.AplicarRegraParaExibirBotaoFazerInscricao(this.usuario, solucaoEducacional, e);
                }

                this.AplicarRegraParaExibirBotaoCertificado(e);
                this.AplicarRegraParaExibirNomeTurma(e);
            }
        }

        private void AplicarRegraParaExibirNomeTurma(GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Label lblTurma = (Label)e.Row.FindControl("btnCertificado");

                if (lblTurma != null)
                {
                    MatriculaOferta matriculaOferta = (MatriculaOferta)e.Row.DataItem;

                    if (matriculaOferta.MatriculaTurma != null &&
                        matriculaOferta.MatriculaTurma.Count > 0)
                    {
                        lblTurma.Text = matriculaOferta.MatriculaTurma[0].Turma.Nome;
                    }

                }
            }
        }

        private void AplicarRegraParaExibirBotaoCertificado(GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Button btnCertificado = (Button)e.Row.FindControl("btnCertificado");

                if (btnCertificado != null)
                {
                    MatriculaOferta matriculaOferta = (MatriculaOferta)e.Row.DataItem;

                    if (matriculaOferta.StatusMatricula.Equals(enumStatusMatricula.Aprovado))
                    {
                        //Se existe um certificado template configurado para a oferta, exibe o botão
                        if (matriculaOferta.Oferta.CertificadoTemplate != null &&
                            matriculaOferta.Oferta.CertificadoTemplate.ID > 0)
                        {
                            btnCertificado.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnCertificado_Click(object sender, EventArgs e)
        {
            LinkButton lbEmitirCertificado = (LinkButton)sender;
            int idMatriculaOferta = int.Parse(lbEmitirCertificado.CommandArgument);
            MatriculaOferta matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(idMatriculaOferta);
            EmitirCertificado(matriculaOferta);
        }

        public void EmitirCertificado(MatriculaOferta matriculaOferta)
        {
            if (matriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID != (int)enumFornecedor.FGVOCW)
            {
                CertificadoTemplate cf = CertificadoTemplateUtil.ConsultarCertificado(0, matriculaOferta.ID, 0);
                DataTable dt = CertificadoTemplateUtil.GerarDataTableComCertificado(0, matriculaOferta.ID, 0, cf);

                string caminhoReport = string.Empty;
                if (string.IsNullOrWhiteSpace(cf.Imagem2) && string.IsNullOrWhiteSpace(cf.TextoCertificado2))
                {
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate.rdlc";
                }
                else
                {
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate2paginas.rdlc";
                }

                string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
                Assembly assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
                Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);


                ReportViewer rv = new ReportViewer();
                rv.LocalReport.LoadReportDefinition(stream);
                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("dsCertificadoTemplate", dt));

                var fileBytes = rv.LocalReport.Render("PDF");

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.AppendHeader("content-length", fileBytes.Length.ToString());
                context.Response.ContentType = "application/pdf";
                context.Response.AppendHeader("content-disposition", "attachment; filename=Certificado.pdf");
                context.Response.BinaryWrite(fileBytes);

                context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                var link = new HistoricoAcademicoServices().ConsultarHistorico(matriculaOferta.Usuario.ID).FirstOrDefault(x => x.IdMatricula == matriculaOferta.ID);

                if (link != null && !string.IsNullOrEmpty(link.LKCertificado))
                {
                    try
                    {
                            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(link.LKCertificado);
                            WebResponse myResp = myReq.GetResponse();
                            HttpContext context = HttpContext.Current;

                            byte[] b = null;
                            using (Stream stream = myResp.GetResponseStream())
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int count = 0;
                                do
                                {
                                    byte[] buf = new byte[1024];
                                    count = stream.Read(buf, 0, 1024);
                                    ms.Write(buf, 0, count);
                                } while (stream.CanRead && count > 0);
                                b = ms.ToArray();

                                context.Response.ContentType = "application/pdf";
                                context.Response.AppendHeader("content-disposition", "attachment; filename=Certificado.pdf");
                                context.Response.BinaryWrite(ms.ToArray());
                                context.Response.Flush();
                            }
                        }
                        catch
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao Baixar Certificado do Servidor Remoto FGV");
                        }

                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Certificado do OCW não encontrado");
                }

            }
        }

        public void LimparGrid()
        {
            IList<SolucaoEducacional> ListaVazia = new List<SolucaoEducacional>();
            WebFormHelper.PreencherGrid(ListaVazia, this.dgvMatriculaOferta);
        }
    }


}