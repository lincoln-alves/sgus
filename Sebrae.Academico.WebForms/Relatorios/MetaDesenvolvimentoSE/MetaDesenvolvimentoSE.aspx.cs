using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.MetaDesenvolvimentoSE
{
    public partial class MetaDesenvolvimentoSE : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (var mdse = new RelatorioMetaDesenvolvimentoSE())
            {
                WebFormHelper.PreencherLista(mdse.ObterNivelOcupacionalTodos(), cbxNivelOcupacional, true);
                WebFormHelper.PreencherLista(mdse.ObterUfTodos(), cbxUF, true);
                WebFormHelper.PreencherLista(mdse.ObterCategoriaConteudoTodos(), cbxCategoriaSE, true);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioMetaDesenvolvimentoSE>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioMetaDesenvolvimentoSE>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var mdse = new RelatorioMetaDesenvolvimentoSE())
            {
                var lstGrid = mdse.ConsultarMetaDesenvolvimentoSe(
                    string.IsNullOrWhiteSpace(cbxNivelOcupacional.SelectedValue)
                        ? 0
                        : int.Parse(cbxNivelOcupacional.SelectedValue),
                    string.IsNullOrWhiteSpace(cbxUF.SelectedValue) ? 0 : int.Parse(cbxUF.SelectedValue),
                    string.IsNullOrWhiteSpace(cbxCategoriaSE.SelectedValue)
                        ? 0
                        : int.Parse(cbxCategoriaSE.SelectedValue));

                Session.Add("dsRelatorio", lstGrid);

                if (lstGrid != null && lstGrid.Count > 0)
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

                dgRelatorio.DataSource = lstGrid;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("MetaDesenvolvimentoSE.rptMetaDesenvolvimentoSE.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }
    }
}