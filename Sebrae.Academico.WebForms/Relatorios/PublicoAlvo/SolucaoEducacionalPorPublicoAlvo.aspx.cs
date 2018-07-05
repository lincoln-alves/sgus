using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.PublicoAlvo
{
    public partial class SolucaoEducacionalPorPublicoAlvo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (var rel = new RelatorioSolucaoEducacionalPublicoAlvo())
            {
                ListBoxesPublicoAlvo.PreencherItens(rel.ObterPublicoAlvoTodos(), "ID", "Nome");
                ListBoxesUFResponsavel.PreencherItens(rel.ObterUFTodos(), "ID", "Nome");
            }

            ucCategorias1.PreencherCategorias(false);
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid(Session["dsRelatorio"] as IList<DTOSolucaoEducacionalPublicoAlvo>, dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid(Session["dsRelatorio"] as IList<DTOSolucaoEducacionalPublicoAlvo>,
                dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var rel = new RelatorioSolucaoEducacionalPublicoAlvo())
            {
                var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

                var lstRelatorio = rel.ConsultarSolucaEducacionalPublicoAlvo(
                    ListBoxesPublicoAlvo.RecuperarIdsSelecionados<int>().ToList(),
                    ucCategorias1.IdsCategoriasMarcadas.ToList(),
                    pUfResponsavel);

                Session.Add("dsRelatorio", lstRelatorio);

                if (lstRelatorio != null && lstRelatorio.Count > 0)
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstRelatorio,
                            "Total da quantidade de SE por públicos alvo", "PublicoAlvo",
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

                dgRelatorio.DataSource = lstRelatorio;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }

        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("SolucaoEducacionalPublicoAlvo.rptSolucaoEducacionalPorPublicoAlvo.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }
    }
}