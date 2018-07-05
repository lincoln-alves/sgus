using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using WebGrease.Css.Extensions;

namespace Sebrae.Academico.WebForms
{
    public partial class MonitoramentoTurma : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            ucCategoriasConteudo.TreeNodeCheckChanged += AtualizarGridTurmas;
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

        public void AtualizarGridTurmas(object sender, EventArgs e)
        {
            var ids = ucCategoriasConteudo.IdsCategoriasMarcadas.ToList();
        }

        protected void btnFiltrar_OnClick(object sender, EventArgs e)
        {
            #region Filtro por categorias
            string categorias = "";
            ucCategoriasConteudo.IdsCategoriasMarcadas.ForEach(x => categorias += x.ToString() + ",");

            if (!string.IsNullOrEmpty(categorias))
            {
                var ultimaVirgula = categorias.LastIndexOf(",");

                // Removendo última ocorrencia de virgula.
                categorias = categorias.Substring(0, ultimaVirgula);
            }
            else
            {
                categorias = null;
            }
            #endregion

            var relMonitoramentoTurmas = new RelatorioMonitoramentoTurmas();
            var turmas = relMonitoramentoTurmas.ObterTodosPorCategoriaConteudo(categorias);
            var totalStatus = relMonitoramentoTurmas.ObterTotalStatus(categorias);

            #region Filtro por data início e data fim.

            DateTime dataInicioTurma;
            if (DateTime.TryParse(txtDataInicialTurma.Text, out dataInicioTurma))
            {
                turmas = turmas.Where(t => t.DataInicio.Date >= dataInicioTurma.Date).ToList();
                totalStatus = totalStatus.Where(t => t.DataInicio.Date >= dataInicioTurma.Date).ToList();
            }

            DateTime dataFimTurma;
            if (DateTime.TryParse(txtDataFinalTurma.Text, out dataFimTurma))
            {
                turmas = turmas.Where(t => t.DataFim.Date <= dataFimTurma.Date).ToList();
                totalStatus = totalStatus.Where(t => t.DataFim.Date <= dataFimTurma.Date).ToList();
            }

            #endregion

            #region Filtro por status

            var listaStatus = chkStatus.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
            turmas = listaStatus.Count > 0 ? turmas.Where(t => t.Status != null && listaStatus.Contains(t.Status.ToString())).ToList() : turmas;
            totalStatus = listaStatus.Count > 0 ? totalStatus.Where(t => t.Status != null && listaStatus.Contains(t.Status.ToString())).ToList() : totalStatus;

            #endregion

            Session["dsRelatorio"] = turmas;

            grdTurma.DataSource = turmas;
            grdTurma.DataBind();

            grdTotalMatriculas.DataSource = totalStatus;
            grdTotalMatriculas.DataBind();
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

            grdTurma.DataSource = listaTurmas;
            grdTurma.DataBind();

            grdTotalMatriculas.DataSource = relMonitoramentoTurmas.ObterTotalStatus();
            grdTotalMatriculas.DataBind();

            Session["dsRelatorio"] = listaTurmas;

        }
    }
}