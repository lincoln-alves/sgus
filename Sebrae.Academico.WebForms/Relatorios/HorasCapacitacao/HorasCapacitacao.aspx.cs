using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.HorasCapacitacao
{
    public partial class HorasCapacitacao : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            using (var bp = new RelatorioHorasCapacitacao())
            {
                WebFormHelper.PreencherLista(bp.ObterPerfilTodos(), cbxPerfil, true);
                WebFormHelper.PreencherLista(bp.ObterUFTodos(), cbxUf, true);
                WebFormHelper.PreencherLista(bp.ObterNivelOcupacionalTodos(), cbxNivelOcupacional, true);
                WebFormHelper.PreencherLista(bp.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, true);
                WebFormHelper.PreencherLista(bp.ObterStatusMatriculaTodos(), cbxStatusMatricula, true);

                ListBoxesUFResponsavel.PreencherItens(bp.ObterUFTodos(), "ID", "Nome");

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(bp.ObterSolucaoEducacionalPorFormaAquisicao());
            }
        }

        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var bp = new RelatorioHorasCapacitacao())
            {
                var idFormaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedItem.Value)
                    ? 0
                    : int.Parse(cbxFormaAquisicao.SelectedItem.Value);

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                var lista = bp.ObterSolucaoEducacionalPorFormaAquisicao(
                    idFormaAquisicao);

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var bp = new RelatorioHorasCapacitacao())
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
                    pUfResponsavel);



                foreach (var checkbox in chkListaCamposVisiveis.Items.Cast<ListItem>().Where(checkbox => !checkbox.Selected))
                {
                    switch (checkbox.Value)
                    {
                        case "UF":
                            consulta =
                                consulta.GroupBy(g => new {g.NivelOcupacional, g.SolucaoEducacional})
                                    .Select(agrupado => new DTORelatorioHorasCapacitacao
                                    {
                                        NivelOcupacional = agrupado.Key.NivelOcupacional,
                                        SolucaoEducacional = agrupado.Key.SolucaoEducacional,
                                        TotalHoras = agrupado.Sum(g => g.TotalHoras)
                                    }).ToList();
                            break;

                        case "NivelOcupacional":
                            consulta =
                                consulta.GroupBy(g => new {g.UF, g.SolucaoEducacional})
                                    .Select(agrupado => new DTORelatorioHorasCapacitacao
                                    {
                                        UF = agrupado.Key.UF,
                                        SolucaoEducacional = agrupado.Key.SolucaoEducacional,
                                        TotalHoras = agrupado.Sum(g => g.TotalHoras)
                                    }).ToList();
                            break;

                        case "SolucaoEducacional":
                            consulta =
                                consulta.GroupBy(g => new {g.UF, g.NivelOcupacional})
                                    .Select(agrupado => new DTORelatorioHorasCapacitacao
                                    {
                                        UF = agrupado.Key.UF,
                                        NivelOcupacional = agrupado.Key.NivelOcupacional,
                                        TotalHoras = agrupado.Sum(g => g.TotalHoras)
                                    }).ToList();
                            break;
                    }
                }

                if (consulta.Any())
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

                lblQuantidadeEncontrada.Text = string.Format("<b>Total Encontrado:</b> {0}",
                    consulta.Sum(x => x.TotalHoras) + " horas");

                dgRelatorio.DataSource = consulta;
                Session.Add("dsRelatorio", consulta);
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }

        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioHorasCapacitacao>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioHorasCapacitacao>)Session["dsRelatorio"],
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

            WebFormHelper.GerarRelatorio("HorasCapacitacao.rptHorasCapacitacao.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }
    }
}