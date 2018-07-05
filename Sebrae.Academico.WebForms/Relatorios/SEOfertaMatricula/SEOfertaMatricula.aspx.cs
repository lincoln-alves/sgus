using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.SEOfertaMatricula
{
    public partial class SEOfertaMatricula : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (var relSEom = new RelatorioSEOfertaMatricula())
            {
                WebFormHelper.PreencherLista(relSEom.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, true);
                WebFormHelper.PreencherLista(relSEom.ObterTipoOfertaTodos(), cbxTipoOferta, true);

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(relSEom.ObterSolucaoEducacional());

                ListBoxesUFResponsavel.PreencherItens(relSEom.ObterUFTodos(), "ID", "Nome");
            }

        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioSEOfertaMatricula>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioSEOfertaMatricula>) Session["dsRelatorio"], dgRelatorio,
                e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var relSEom = new RelatorioSEOfertaMatricula())
            {
                var idFormaAquisicao = int.Parse(cbxFormaAquisicao.SelectedValue);

                var lista = relSEom.ObterSolucaoEducacionalPorFormaAquisicao(null, idFormaAquisicao);

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relSeBm = new RelatorioSEOfertaMatricula())
            {
                var formaDeAquisicaoId = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue)
                    ? null
                    : (int.Parse(cbxFormaAquisicao.SelectedValue) == 0
                        ? null
                        : (int?) int.Parse(cbxFormaAquisicao.SelectedValue));

                var solucaoEducacionalId = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text)
                    ? null
                    : (int.Parse(txtSolucaoEducacional.Text) == 0
                        ? null
                        : (int?) int.Parse(txtSolucaoEducacional.Text));

                var tipoOfertaId = string.IsNullOrWhiteSpace(cbxTipoOferta.SelectedValue)
                    ? null
                    : (int.Parse(cbxTipoOferta.SelectedValue) == 0
                        ? null
                        : (int?) int.Parse(cbxTipoOferta.SelectedValue));

                var ufsResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>().ToList();

                DateTime? dtIni = null;
                DateTime? dtFim = null;

                DateTime dtIniAux, dtFimAux;

                if (!string.IsNullOrWhiteSpace(txtDataInicio.Text) && DateTime.TryParse(txtDataInicio.Text, out dtIniAux))
                    dtIni = dtIniAux;

                if (!string.IsNullOrWhiteSpace(txtDataFim.Text) && DateTime.TryParse(txtDataFim.Text, out dtFimAux))
                    dtFim = dtFimAux;

                var lstGrid =
                    relSeBm.ConsultarSeOfertaMatricula(formaDeAquisicaoId, solucaoEducacionalId, tipoOfertaId, dtIni,
                        dtFim, ufsResponsavel)
                        .OrderBy(x => x.SolucaoEducacional)
                        .ThenBy(x => x.Oferta)
                        .ToList();

                if (lstGrid.Any())
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade de ofertas", "ID_Oferta",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade por tipo de oferta",
                            "TipoOferta", enumTotalizacaoRelatorio.Contar)
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

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("SEOfertaMatricula.rptSEOfertaMatricula.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }
    }
}