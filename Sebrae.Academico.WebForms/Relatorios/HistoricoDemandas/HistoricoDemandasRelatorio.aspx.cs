using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Services.Processo;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Text.RegularExpressions;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;
using ManterProcesso = Sebrae.Academico.BP.Services.SgusWebService.ManterProcesso;

namespace Sebrae.Academico.WebForms.Relatorios.HistoricoDemandas
{
    public partial class HistoricoDemandasRelatorio : System.Web.UI.Page
    {
        private DTOAnalisarEtapasProcesso analisarEtapasProcesso;
        private ProcessoResposta processo;

        protected void Page_Load(object sender, EventArgs e)
        {

            var id = int.Parse(Request.QueryString["demanda"]);
            processo = new BMProcessoResposta().ObterPorId(id);

            analisarEtapasProcesso = new ManterProcesso().AnalisarEtapasProcesso(id, processo.Usuario.CPF);

            if (analisarEtapasProcesso != null && analisarEtapasProcesso.ListaEtapas != null && processo != null)
            {
                rptEtapas.DataSource = analisarEtapasProcesso.ListaEtapas.ToList();
                rptEtapas.DataBind();
                printPdfFromHtml();
            }
           
        }
        protected void rptEtapas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var etapa = (DTOEtapa)e.Item.DataItem;
            var lblTitle = (Label)e.Item.FindControl("lblTitle");
            var lblDemandadoPro = (Label)e.Item.FindControl("lblDemandadoPor");
            var lblDataSolicitacao = (Label)e.Item.FindControl("lblDataSolicitacao");
            var lblNumeroDemanda = (Label)e.Item.FindControl("lblNumeroDemanda");
            lblTitle.Text = "#"+processo.ID + " - "+ processo.Processo.Nome;
            lblDemandadoPro.Text = processo.Usuario.Nome;
            lblDataSolicitacao.Text = processo.DataSolicitacao.ToString();
            lblNumeroDemanda.Text = processo.ID.ToString();


            var resposta = new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(etapa.ID_RespostaEtapa);

            var rptResposta = (Repeater)e.Item.FindControl("rptResposta");
            if (resposta != null)
            {
                rptResposta.DataSource = resposta.ListaItemQuestionarioParticipacao.Select(p => new { Resposta = this.CleanHtml(p.Resposta), Questao = p.Questao });
                rptResposta.DataBind();
            }
        }

        protected string CleanHtml(string str)
        {
            return String.IsNullOrWhiteSpace(str) ? String.Empty : Regex.Replace(str, @"<[^>]*>", String.Empty);
        }

        protected void rptResposta_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
          
        }

        private void printPdfFromHtml ()
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter(sb));
            base.Render(tw);

            Document document = new Document();
            var streamOutput = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, streamOutput);

            document.Open();

            using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("")))
            {
                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString())))
                {
                    try
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml, msCss);
                    }
                    catch (Exception ex)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                        return;
                    }

                }
            }

         
            document.Close();

            Response.AddHeader("Content-Disposition",
                            "attachment; filename=historicoDemanda_" + processo.ID + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(streamOutput.ToArray());
            Response.Flush();
            streamOutput.Close();
            Response.End();
        }

        
    }
}