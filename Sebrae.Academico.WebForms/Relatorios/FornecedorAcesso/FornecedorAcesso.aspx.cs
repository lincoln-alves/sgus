using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;

namespace Sebrae.Academico.WebForms.Relatorios.FornecedorAcesso
{
    public partial class FornecedorAcesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid<DTORelatorioFornecedorAcesso>((IList<DTORelatorioFornecedorAcesso>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid<DTORelatorioFornecedorAcesso>((IList<DTORelatorioFornecedorAcesso>)Session["dsRelatorio"], dgRelatorio,
                                                                         e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (RelatorioFornecedorAcesso relFornAcess = new RelatorioFornecedorAcesso())
            {

                IList<DTORelatorioFornecedorAcesso> lstGrid = relFornAcess.ConsultarForneceorAcesso(txtNome.Text);

                Session.Add("dsRelatorio", lstGrid);

                dgRelatorio.DataSource = lstGrid;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);

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

            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("FornecedorAcesso.rptFornecedorAcesso.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }
    }
}