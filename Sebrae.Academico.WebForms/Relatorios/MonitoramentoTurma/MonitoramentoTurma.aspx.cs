using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.MonitoramentoTurma
{
    public partial class MonitoramentoTurma : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            ucCategoriasConteudo.TreeNodeCheckChanged += AtualizarCategorias;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var tituloControl = Page.Master != null ? (Literal)Page.Master.FindControl("ltnTitulo") : null;

            if (Page.IsPostBack) return;

            if (tituloControl != null)
            {
                tituloControl.Text = "Monitoramento de Turmas";
            }

            PreencherCampos();
        }

        public void AtualizarCategorias(object sender, EventArgs e)
        {
            AtualizarComboSolucaoEducacional(null, null);
        }

        protected void btnFiltrar_OnClick(object sender, EventArgs e)
        {
            #region Filtro por categorias

            var listaIds = ucCategoriasConteudo.IdsCategoriasMarcadas;
            var categorias = listaIds.Aggregate("", (current, id) => current + (id + ","));

            categorias = !string.IsNullOrEmpty(categorias) ? categorias.Substring(0, categorias.Length) : null;
            #endregion

            var relMonitoramentoTurmas = new RelatorioMonitoramentoTurmas();
            var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

            var turmas = relMonitoramentoTurmas.ObterTodosPorCategoriaConteudo(categorias, pUfResponsavel);

            #region Filtro por data início e data fim.

            var pDataInicio = (DateTime?) null;
            DateTime dataInicioTurma;
            if (DateTime.TryParse(txtDataInicialTurma.Text, out dataInicioTurma))
            {
                turmas = turmas.Where(t => t.DataInicio.Date >= dataInicioTurma.Date).ToList();
                pDataInicio = dataInicioTurma;
            }

            var pDataFim = (DateTime?)null;
            DateTime dataFimTurma;
            if (DateTime.TryParse(txtDataFinalTurma.Text, out dataFimTurma))
            {
                turmas = turmas.Where(t => t.DataFim.Date <= dataFimTurma.Date).ToList();
                pDataFim = dataFimTurma;
            }

            #endregion

            #region Filtro por status

            var listaStatus = chkStatus.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
            turmas = listaStatus.Count > 0 ? turmas.Where(t => t.Status != null && listaStatus.Contains(t.Status.ToString())).ToList() : turmas;

            #endregion

            var status = listaStatus.Aggregate("", (current, id) => current + (id + ","));
            status = !string.IsNullOrEmpty(status) ? status.Substring(0, status.Length) : null;
            var totalStatus = relMonitoramentoTurmas.ObterTotalStatus(categorias, pDataInicio, pDataFim, status, pUfResponsavel);

            Session["dsRelatorio"] = turmas;

            grdTurma.DataSource = turmas;
            grdTurma.DataBind();
            grdTurma.Visible = true;

            grdTotalMatriculas.DataSource = totalStatus;
            grdTotalMatriculas.DataBind();
            grdTotalMatriculas.Visible = true;

            pnl1.Visible = true;
        }

        protected void btnGerarRelatorio_OnClick(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnFiltrar_OnClick(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("MonitoramentoTurma.rptMonitoramentoTurma.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, null);
        }

        public void PreencherCampos()
        {
            var relMonitoramentoTurmas = new RelatorioMonitoramentoTurmas();
            ucCategoriasConteudo.PreencherTodasCategorias(false, null, null, false, true);

            var listaStatus = Enum.GetValues(typeof(enumStatusTurma)).Cast<enumStatusTurma>().Select(e => new { nome = e.GetDescription(), valor = (int)e });
            WebFormHelper.PreencherListaCustomizado(listaStatus.ToList(), chkStatus, "valor", "nome");

            var listaTurmas = relMonitoramentoTurmas.ObterTodosPorCategoriaConteudo();

            ListBoxesUFResponsavel.PreencherItens(relMonitoramentoTurmas.ObterUFTodos(), "ID", "Nome");
        }

        public void AtualizarComboSolucaoEducacional(object sender, EventArgs e)
        {
            var categoria = ucCategoriasConteudo.IdsCategoriasMarcadas.ToList();
            var manterSe = new ManterSolucaoEducacional();
            var solucoes = categoria.Any() ? manterSe.ObterTodosSolucaoEducacional().Where(s => s.CategoriaConteudo != null && categoria.Contains(s.CategoriaConteudo.ID)) :
                manterSe.ObterTodosSolucaoEducacional();

            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(solucoes);

            // Limpar SE, oferta e turma selecionadas.
            //txtSolucaoEducacional.Text = "";
        }

        protected void grdTurma_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var lista = (List<DTOMonitoramentoTurma>)Session["dsRelatorio"];
            if (lista != null)
                WebFormHelper.PaginarGrid(lista, grdTurma, e.NewPageIndex);
        }
    }
}