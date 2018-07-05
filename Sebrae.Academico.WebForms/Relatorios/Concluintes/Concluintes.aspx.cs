using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.Concluintes
{
    public partial class Concluintes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var rel = new RelatorioConcluintes())
                {
                    WebFormHelper.PreencherLista(rel.ObterListaFormaAquisicao(), cbxTipoCurso, true);
                    WebFormHelper.PreencherLista(rel.ObterListaUf(), cbxUF, true);

                    ListBoxesUFResponsavel.PreencherItens(rel.ObterListaUf(), "ID", "Nome");
                }
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTOConcluinte>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTOConcluinte>) Session["dsRelatorio"],
                dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var rel = new RelatorioConcluintes())
            {
                var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

                var lstGrid = rel.ObterRelatorioConcluinte(
                    cbxTipoCurso.SelectedIndex > 0 ? (int?)int.Parse(cbxTipoCurso.SelectedValue) : null,
                    cbxUF.SelectedIndex > 0 ? (int?)int.Parse(cbxUF.SelectedValue) : null, pUfResponsavel);

                Session.Add("dsRelatorio", lstGrid);

                if (lstGrid != null && lstGrid.Count > 0)
                {
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total de registros", "Concluintes",
                            enumTotalizacaoRelatorio.Contar, false)
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

                dgRelatorio.DataSource = lstGrid;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("Concluintes.Concluintes.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida,
                chkListaCamposVisiveis.Items);
        }
    }
}