using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.QuestionarioAvaliacao
{
    public partial class QuestionarioAvaliacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (RelatorioQuestionarioAvaliacao rel = new RelatorioQuestionarioAvaliacao())
                {
                    WebFormHelper.PreencherLista(rel.ObterQuestionario(), cbxNomeQuestionario, true, false);
                    WebFormHelper.PreencherLista(rel.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, true, false);
                    WebFormHelper.PreencherLista(rel.ObterSolucaoEducacional(), cbxSolucaoEducacional, true, false);
                }
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid<DTOQuestionarioAvaliacao>(Session["dsRelatorio"] as IList<DTOQuestionarioAvaliacao>, dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid(Session["dsRelatorio"] as IList<DTOQuestionarioAvaliacao>, dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (RelatorioQuestionarioAvaliacao rel = new RelatorioQuestionarioAvaliacao())
            {
                ItemTrilha pItemTrilha = new ItemTrilha();
                //if (cbxNomeQuestionario.SelectedIndex> 0)
                //    pItemTrilha.
                if (cbxFormaAquisicao.SelectedIndex > 0)
                    pItemTrilha.FormaAquisicao.ID = int.Parse(cbxFormaAquisicao.SelectedValue);
                if (cbxSolucaoEducacional.SelectedIndex > 0)
                    pItemTrilha.SolucaoEducacional.ID = int.Parse(cbxSolucaoEducacional.SelectedValue);

                IList<DTOQuestionarioAvaliacao> lstRelatorio = rel.ConsultarQuestionarioAvaliacao(pItemTrilha);

                Session.Add("dsRelatorio", lstRelatorio);

                if (lstRelatorio != null && lstRelatorio.Count > 0)
                {
                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                dgRelatorio.DataSource = lstRelatorio;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("SolucaoEducacionalFormaAquisicao.rptSolucaoEducacionalFormaAquisicao.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }
    }
}