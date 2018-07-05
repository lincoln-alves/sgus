using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Relatorios.InscricoesPorStatusENivel
{
    public partial class InscricoesPorStatusENivel : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            using (var bp = new RelatorioInscricoesPorStatusENivel())
            {
                var listaUfs = bp.ObterUFs();
                WebFormHelper.PreencherLista(listaUfs, cbxUf, listaUfs.Count > 1);

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(bp.ObterSolucaoEducacionalTodos());

                CheckboxesStatus.PreencherItens(bp.ObterStatusMatriculaTodos(), "ID", "Nome", true);
                CheckboxesNiveis.PreencherItens(bp.ObterNivelOcupacionalTodos(), "ID", "Nome", true);

                ListBoxesUFResponsavel.PreencherItens(listaUfs, "ID", "Nome");
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var bp = new RelatorioInscricoesPorStatusENivel())
            {
                DateTime dtIni, dtFim;
                DateTime? dtIniConvertido = null, dtFimConvertido = null;
                if (DateTime.TryParse(txtDataInicio.Text, out dtIni))
                    dtIniConvertido = dtIni;

                if (DateTime.TryParse(txtDataFim.Text, out dtFim))
                    dtFimConvertido = dtFim;

                var statuses = CheckboxesStatus.RecuperarIdsSelecionados<int>().ToList();

                var niveis = CheckboxesNiveis.RecuperarIdsSelecionados<int>().ToList();

                var pStatus = statuses.Any() ? string.Join(",", statuses) : null;

                var pNiveis = niveis.Any() ? string.Join(",", niveis) : null;

                var pUf = string.IsNullOrWhiteSpace(cbxUf.SelectedValue)
                    ? null
                    : int.Parse(cbxUf.SelectedValue) == 0 ? null : (int?) int.Parse(cbxUf.SelectedValue);

                var pSe = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text)
                    ? null
                    : int.Parse(txtSolucaoEducacional.Text) == 0 ? null : (int?) int.Parse(txtSolucaoEducacional.Text);

                var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

                var consulta = bp.ConsultarRelatorio(
                    pStatus,
                    pNiveis,
                    pUf,
                    pSe,
                    dtIniConvertido,
                    dtFimConvertido,
                    pUfResponsavel);

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

                lblQuantidadeEncontrada.Text = string.Format("<b>Total Encontrado:</b> {0}",
                    (consulta != null && consulta.Any() ? consulta.Sum(r => r.Total) : 0));

                dgRelatorio.DataSource = consulta;
                Session.Add("dsRelatorio", consulta);

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, CheckboxesNiveis.Items);
            }

        }


        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTOProcInscricoesPorStatusENivel>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTOProcInscricoesPorStatusENivel>) Session["dsRelatorio"], dgRelatorio,
                e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            //Usando as colunas da gridview para gerar os parâmetros do relatório
            var collection = new ListItemCollection();
            dgRelatorio.Columns.Cast<BoundField>()
                .Select(c => new ListItem() {Value = c.DataField, Selected = c.Visible})
                .ToList().ForEach(c => collection.Add(c));

            WebFormHelper.GerarRelatorio("InscricoesPorStatusENivel.rptInscricoes.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, collection);
        }

        protected void dgRelatorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    var sessionName = "RegistrosInscricoesPorStatus_" + e.Row.Cells.GetCellIndex(cell);

                    //Reiniciando sessão sempre que começar a contagem
                    if (e.Row.RowIndex == 0)
                        Session[sessionName] = null;

                    int valorTotal = 0;

                    if (Session[sessionName] != null)
                        valorTotal = Convert.ToInt32(Session[sessionName]);

                    int valorCelula;
                    if (int.TryParse(cell.Text, out valorCelula))
                    {
                        Session[sessionName] = valorTotal + valorCelula;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";

                foreach (TableCell cell in e.Row.Cells)
                {
                    var sessionName = "RegistrosInscricoesPorStatus_" + e.Row.Cells.GetCellIndex(cell);

                    if (Session[sessionName] != null)
                    {
                        cell.Text = Session[sessionName].ToString();
                    }
                }
            }
        }
    }
}