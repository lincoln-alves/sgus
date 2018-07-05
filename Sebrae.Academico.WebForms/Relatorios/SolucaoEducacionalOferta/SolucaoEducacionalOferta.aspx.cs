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

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class SolucaoEducacionalOferta : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            
            using (var relSeo = new RelatorioSolucaoEducacionalOferta())
            {
                WebFormHelper.PreencherLista(relSeo.ObterTipoOfertaTodos(), cbxTipoOferta, true);
                WebFormHelper.PreencherLista(relSeo.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, true);

                var lista = relSeo.ObterSolucaoEducacionalPorFormaAquisicao();

                ListBoxesUFResponsavel.PreencherItens(relSeo.ObterUFTodos(), "ID", "Nome");

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }

            //Task #416 - revisão de regras de visualização das UFs
            var ufs = new ManterUf().ObterTodosUf();
            WebFormHelper.PreencherLista(ufs, cbxUF);
        }

        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var relSeo = new RelatorioSolucaoEducacionalOferta())
            {
                var idForma = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue)
                    ? 0
                    : int.Parse(cbxFormaAquisicao.SelectedValue);

                var lista = relSeo.ObterSolucaoEducacionalPorFormaAquisicao(null, idForma);

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            using (var relSo = new RelatorioSolucaoEducacionalOferta())
            {
                var idTipoOferta = string.IsNullOrWhiteSpace(cbxTipoOferta.SelectedValue)
                    ? null
                    : (int.Parse(cbxTipoOferta.SelectedValue) == 0
                        ? null
                        : (int?) int.Parse(cbxTipoOferta.SelectedValue));

                var idFormaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue)
                    ? null
                    : (int.Parse(cbxFormaAquisicao.SelectedValue) == 0
                        ? null
                        : (int?) int.Parse(cbxFormaAquisicao.SelectedValue));

                var ufsResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

                var idSolucaoEducacional = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text)
                    ? null
                    : (int.Parse(txtSolucaoEducacional.Text) == 0 ? null : (int?) int.Parse(txtSolucaoEducacional.Text));

                //Task #416 - revisão de regras de visualização das UFs
                var pUf = string.IsNullOrWhiteSpace(cbxUF.SelectedValue) ? null : (int.Parse(cbxUF.SelectedValue) == 0 ? null : (int?)int.Parse(cbxUF.SelectedValue));

                var lstRelatorio = relSo.ConsultarSolucaoEducacionalOferta(idFormaAquisicao, idTipoOferta,
                    idSolucaoEducacional, pUf, ufsResponsavel);

                Session.Add("dsRelatorio", lstRelatorio);

                if (lstRelatorio != null && lstRelatorio.Count > 0)
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstRelatorio, "Total de registros (soluções educacionais)",
                            "ID_SolucaoEducacional", enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lstRelatorio,
                            "Total da quantidade de ofertas separadas por tipo de oferta", "TipoOferta",
                            enumTotalizacaoRelatorio.Contar)
                    };

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);

                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnConsultar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }


                dgRelatorio.DataSource = lstRelatorio;

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTOSolucaoEducacionalOferta>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTOSolucaoEducacionalOferta>) Session["dsRelatorio"], dgRelatorio,
                e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnConsultar_Click(null, null);
            
            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("SolucaoEducacionalOferta.rptSolucaoEducacionalOferta.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }
    }
}