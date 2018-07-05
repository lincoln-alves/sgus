using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.SolucaoEducacionalPrograma
{
    public partial class SolucaoEducacionalPrograma : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (var relProg = new RelatorioSolucaoEducacionalPrograma())
            {
                WebFormHelper.PreencherLista(relProg.ObterProgramaTodos(), cbxPrograma, true, false);
                ListBoxesUFResponsavel.PreencherItens(relProg.ObterUFTodos(), "ID", "Nome");
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relProg = new RelatorioSolucaoEducacionalPrograma())
            {
                var ufsResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();
                var lstGrid =
                    relProg.ObterSolucaoEducacionalPorPrograma(
                        string.IsNullOrWhiteSpace(cbxPrograma.SelectedValue) ? 0 : int.Parse(cbxPrograma.SelectedValue), ufsResponsavel);

                if (lstGrid != null && lstGrid.Count > 0)
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstGrid,
                            "Total da quantidade de SE's separadas por programa", "Programa",
                            enumTotalizacaoRelatorio.Contar)
                    };

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);

                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                Session.Add("dsRelatorio", lstGrid);

                dgRelatorio.DataSource = lstGrid;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioSolucaoEducacionalPrograma>) Session["dsRelatorio"],
                dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioSolucaoEducacionalPrograma>) Session["dsRelatorio"],
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

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("SolucaoEducacionalPrograma.rptRelatorioSolucaoEducacionalPrograma.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }
    }
}