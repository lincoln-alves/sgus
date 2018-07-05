using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class DadosProfessor : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Task #416 - revisão de regras de visualização das UFs
            if (!IsPostBack)
            {
                var ufs = new ManterUf().ObterTodosUf();
                WebFormHelper.PreencherLista(ufs, cbxUF);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relProf = new RelatorioDadosProfessor())
            {
                //parametros
                //Task #416 - revisão de regras de visualização das UFs
                var pNome = string.IsNullOrWhiteSpace(txtNome.Text) ? null : txtNome.Text;
                var pCpf = string.IsNullOrWhiteSpace(txtCPF.Text) ? null : txtCPF.Text.Replace("-", "").Replace(".", "");
                var pUf = string.IsNullOrWhiteSpace(cbxUF.SelectedValue) ? null : (int.Parse(cbxUF.SelectedValue) == 0 ? null : (int?)int.Parse(cbxUF.SelectedValue));

                var lstGrid = relProf.ConsultarDadosProfessor(
                    pNome,
                    pCpf,
                    pUf);

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

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioDadosProfessor>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioDadosProfessor>) Session["dsRelatorio"],
                dgRelatorio,
                e.SortExpression,
                e.SortDirection,
                "dsRelatorio");
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("DadosProfessor.rptDadosProfessor.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida,
                chkListaCamposVisiveis.Items);
        }
    }
}