using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.Capacitados
{
    public partial class Capacitados : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            
            using (var bp = new RelatorioCapacitados())
            {
                WebFormHelper.PreencherLista(bp.ObterPerfilTodos(), cbxPerfil, true);
                WebFormHelper.PreencherLista(bp.ObterUFTodos(), cbxUf, true);
                WebFormHelper.PreencherLista(bp.ObterNivelOcupacionalTodos(), cbxNivelOcupacional, true);
                WebFormHelper.PreencherLista(bp.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, true);
                WebFormHelper.PreencherLista(bp.ObterStatusMatriculaTodos(), cbxStatusMatricula, true);
                WebFormHelper.PreencherLista(
                    new ListItemCollection
                    {
                        new ListItem("Todos", ""),
                        new ListItem("Ativo", "ativo"),
                        new ListItem("Inativo", "inativo")
                    }, cbxSituacaoUsuario);

                ListBoxesUFResponsavel.PreencherItens(bp.ObterUFTodos(), "ID", "Nome");

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(bp.ObterSolucaoEducacionalPorFormaAquisicao());
            }
        }

        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var bp = new RelatorioCapacitados())
            {
                var idFormaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedItem.Value)
                    ? 0
                    : int.Parse(cbxFormaAquisicao.SelectedItem.Value);

                var lista = bp.ObterSolucaoEducacionalPorFormaAquisicao(idFormaAquisicao);

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var bp = new RelatorioCapacitados())
            {
                DateTime dtIni, dtFim;
                DateTime? dtIniConvertido = null, dtFimConvertido = null;
                if (DateTime.TryParse(txtDataInicio.Text, out dtIni))
                    dtIniConvertido = dtIni;

                if (DateTime.TryParse(txtDataFim.Text, out dtFim))
                    dtFimConvertido = dtFim;

                var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

                var consulta = bp.ConsultarRelatorio(
                    string.IsNullOrWhiteSpace(cbxPerfil.SelectedValue)
                        ? null
                        : (int?) int.Parse(cbxPerfil.SelectedValue),
                    string.IsNullOrWhiteSpace(cbxUf.SelectedValue) ? null : (int?) int.Parse(cbxUf.SelectedValue),
                    string.IsNullOrWhiteSpace(cbxNivelOcupacional.SelectedValue)
                        ? null
                        : (int?) int.Parse(cbxNivelOcupacional.SelectedValue),
                    string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue)
                        ? null
                        : (int?) int.Parse(cbxFormaAquisicao.SelectedValue),
                    string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text)
                        ? null
                        : (int?) int.Parse(txtSolucaoEducacional.Text),
                    string.IsNullOrWhiteSpace(cbxStatusMatricula.SelectedValue)
                        ? null
                        : (int?) int.Parse(cbxStatusMatricula.SelectedValue),
                    dtIniConvertido,
                    dtFimConvertido,
                    !string.IsNullOrEmpty(cbxSituacaoUsuario.SelectedValue)
                        ? cbxSituacaoUsuario.SelectedValue.ToLower()
                        : null,
                    pUfResponsavel);

                var camposDesmarcados =
                    chkListaCamposVisiveis.Items.Cast<ListItem>().Where(c => !c.Selected).Select(c => c.Value);

                consulta = bp.AgruparRegistros(consulta, camposDesmarcados);

                if (consulta != null && consulta.Any())
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

                lblQuantidadeEncontrada.Text = string.Format("<b>Total Encontrado:</b> {0} pessoas capacitadas<br>",
                    consulta.Count());
                lblQuantidadeEncontrada.Text += string.Format("<b>Total Encontrado:</b> {0} registros de capacitados",
                    consulta.Sum(x => x.TotalCapacitados));

                dgRelatorio.DataSource = consulta;
                Session.Add("dsRelatorio", consulta);
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioCapacitados>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioCapacitados>) Session["dsRelatorio"],
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

            WebFormHelper.GerarRelatorio("Capacitados.rptCapacitados.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida,
                chkListaCamposVisiveis.Items);
        }
    }
}