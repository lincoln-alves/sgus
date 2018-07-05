using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.Processo;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using ManterProcesso = Sebrae.Academico.BP.Services.SgusWebService.ManterProcesso;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.HistoricoDemandas
{
    public partial class Detalhes : System.Web.UI.Page
    {
        private string txtCancelamento = null;
        private ProcessoResposta processo;
        private int Id
        {
            get
            {
                return ViewState["idProcesso"] != null ? int.Parse(ViewState["idProcesso"].ToString()) : 0;
            }
            set
            {
                ViewState["idProcesso"] = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["demanda"] == null)
                {
                    Response.Redirect("HistoricoDemandas.aspx");
                    return;
                }

                Id = int.Parse(Request.QueryString["demanda"]);

                processo = new BMProcessoResposta().ObterPorId(Id);

                var analisarEtapasProcesso = new ManterProcesso().AnalisarEtapasProcesso(Id, processo.Usuario.CPF);
                //lblJustificativaCancelamentoRodape.Text = "";


                lblProcesso.Text = processo.Processo.Nome;
                lblDemandante.Text = processo.Usuario.Nome;
                lblNumeroDemanda.Text = processo.ID.ToString();

                if (!string.IsNullOrEmpty(processo.JustificativaCancelamento))
                {
                    pnlJustificativa.Visible = true;
                    txtCancelamento = lblJustificativaCancelamento.Text = processo.JustificativaCancelamento;
                }

                ViewState["larguraCampo"] = 0;

                //Reapeat Abas
                rptEtapasTab.DataSource = analisarEtapasProcesso.ListaEtapas;
                rptEtapasTab.DataBind();

                //Repeat Content
                rptEtapas.DataSource = analisarEtapasProcesso.ListaEtapas;
                rptEtapas.DataBind();
            }
        }

        protected void rptEtapas_OnAbaDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var rptFormulario = (Repeater)e.Item.FindControl("rptFormulario");
                var etapaResposta = (DTOEtapa)e.Item.DataItem;
                var campos = new BMCampoResposta().ObterPorEtapaRespostaId(etapaResposta.ID_RespostaEtapa);
                var divDataPreenchimento = (HtmlGenericControl)e.Item.FindControl("divDataPreenchimento");

                var panelCancelamentoRodape = (Panel)e.Item.FindControl("pnlJustificativaRodape");
                var labelCancelamentoRodape = (Label)e.Item.FindControl("lblJustificativaCancelamentoRodape");

                if (etapaResposta.Status == (int)enumStatusEtapaResposta.Cancelado)
                {
                    labelCancelamentoRodape.Text = txtCancelamento;
                    panelCancelamentoRodape.Visible = true;
                }

                if (!string.IsNullOrWhiteSpace(etapaResposta.DataPreenchimento))
                {
                    divDataPreenchimento.Visible = true;
                }

                rptFormulario.DataSource = campos;
                rptFormulario.DataBind();
            }
        }

        protected void rptEtapas_OnItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var rptFormulario = (Repeater)e.Item.FindControl("rptFormulario");
                var etapaResposta = (DTOEtapa)e.Item.DataItem;
                var campos = new BMCampoResposta().ObterPorEtapaRespostaId(etapaResposta.ID_RespostaEtapa);
                var divDataPreenchimento = (HtmlGenericControl)e.Item.FindControl("divDataPreenchimento");

                var panelCancelamentoRodape = (Panel)e.Item.FindControl("pnlJustificativaRodape");
                var labelCancelamentoRodape = (Label)e.Item.FindControl("lblJustificativaCancelamentoRodape");

                if (etapaResposta.Status == (int)enumStatusEtapaResposta.Cancelado)
                {
                    labelCancelamentoRodape.Text = txtCancelamento;
                    panelCancelamentoRodape.Visible = true;
                }

                if (!string.IsNullOrWhiteSpace(etapaResposta.DataPreenchimento))
                {
                    divDataPreenchimento.Visible = true;
                }

                rptFormulario.DataSource = campos;
                rptFormulario.DataBind();
            }
        }

        protected void rptFormulario_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) ||
                (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var lblNome = (Label)e.Item.FindControl("lblNomeCampo");
                var lblResposta = (Label)e.Item.FindControl("lblResposta");
                var divAbrirCampo = (HtmlGenericControl)e.Item.FindControl("divAbrirCampo");
                var lblQuebrarLinha = (Label)e.Item.FindControl("lblQuebrarLinha");
                var campo = (CampoResposta)e.Item.DataItem;
                var checkListAlternativas = (CheckBoxList)e.Item.FindControl("chkListaAlternativas");

                var largura = int.Parse(ViewState["larguraCampo"].ToString());

                largura += campo.Campo.Largura;

                if (largura > 12)
                {
                    lblQuebrarLinha.Visible = true;
                    lblQuebrarLinha.Text = "</div><div class=\"row\">";
                    largura = campo.Campo.Largura;
                }

                divAbrirCampo.Attributes.Add("class", "col-md-" + campo.Campo.Largura + " col-xs-12");

                ViewState["larguraCampo"] = largura;

                lblNome.Text = campo.Campo.Nome;
                ManterCampo mCampo = new ManterCampo();
                // Se tiver alternativas mostra todas e marca quais foram escolhidas
                if (campo.Campo.ListaAlternativas.Count() > 0)
                {
                    var alternativasRespondidas = new BMAlternativaResposta().ObterPorCampoRespostaId(campo.ID);
                    var count = 0;
                    foreach (var alternativa in campo.Campo.ListaAlternativas)
                    {

                        checkListAlternativas.Items.Add(new System.Web.UI.WebControls.ListItem(alternativa.Nome, "0", false));

                        if (alternativasRespondidas.Count(d => d.Alternativa == alternativa) > 0)
                        {
                            checkListAlternativas.Items[count].Selected = true;
                        }
                        count++;

                    }
                }
                else
                {
                    // Caso seja um capo do tipo somatório recupera o total da soma
                    if (campo.Campo.TipoCampo == (int)enumTipoCampo.Somatório &&
                        campo.Campo.ListaCamposVinculados.Count() > 0)
                    {
                        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
                        lblResposta.Text = mCampo.ObterTotalSomatorio(campo.Campo, campo.EtapaResposta.ProcessoResposta.ID).ToString(culture);
                    }
                    else if (campo.Campo.TipoCampo == (int)enumTipoCampo.Questionário)
                    {
                        int idResposta;
                        QuestionarioParticipacao resposta;

                        if (int.TryParse(campo.Resposta, out idResposta))
                        {
                            resposta = new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(idResposta);
                            var rptRespostasQuestionario = (Repeater)e.Item.FindControl("rptRespostasQuestionario");
                            if (resposta != null)
                            {
                                rptRespostasQuestionario.DataSource = resposta.ListaItemQuestionarioParticipacao;
                                rptRespostasQuestionario.DataBind();
                            }
                        }
                    }
                    else
                    {
                        lblResposta.Text = campo.Resposta;
                    }
                }

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("HistoricoDemandas.aspx");
        }

        private class DtoBlocoCampo
        {
            public DtoBlocoCampo(bool possuiEspacoAntes, int alturaMinima)
            {
                PossuiEspacoAntes = possuiEspacoAntes;
                Campos = new List<DTOCampo>();

                // Altura mínima para caso só tenha um campo.
                AlturaBloco = alturaMinima;
            }

            public int AlturaBloco { get; set; }
            public List<DTOCampo> Campos { get; set; }
            public bool PossuiEspacoAntes { get; set; }
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/HistoricoDemandas/HistoricoDemandasRelatorio.aspx?demanda="+Id);
        }

        public string GetDescription(enumStatusEtapaResposta en)
        {
            return EnumExtensions.GetDescription(en);
        }
      
    }
}