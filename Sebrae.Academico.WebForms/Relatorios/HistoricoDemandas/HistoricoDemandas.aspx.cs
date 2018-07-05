using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.WebForms.Relatorios.HistoricoDemandas
{
    public partial class HistoricoDemandas : System.Web.UI.Page
    {
        private string txtCancelamento = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                HttpCookie filtroCookie = Request.Cookies["activeFiltroGroup"];
                if (filtroCookie != null)
                {
                    Filtros.CssClass = "panel-collapse collapse in";
                }

                return;
            }

            var ufs = new ManterUf().ObterTodosUf();

            ListBoxesUF.PreencherItens(ufs, "ID", "Nome");

            using (var bm = new BMProcesso())
            {
                WebFormHelper.PreencherLista(bm.ObterTodos(), cbxProcesso, true, false);
            }
        }


        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var bm = new BMProcesso())
            {
                DateTime dtIni, dtFim;
                DateTime? dtIniConvertido = null, dtFimConvertido = null;
                if (DateTime.TryParse(txtDataInicio.Text, out dtIni))
                    dtIniConvertido = dtIni;

                if (DateTime.TryParse(txtDataFim.Text, out dtFim))
                    dtFimConvertido = dtFim;

                int idProcessoresposta = 0;
                int.TryParse(txtProcessoResposta.Text, out idProcessoresposta);

                var ufs = ListBoxesUF.RecuperarIdsSelecionados<int>().ToList();

                int idEtapa = 0;
                int.TryParse(cbxDemanda.SelectedValue, out idEtapa);

                var usuario = ucLupaUsuario.SelectedUser;

                var consulta = bm.BuscarHistorico(!string.IsNullOrWhiteSpace(cbxProcesso.SelectedValue) ? (int?)int.Parse(cbxProcesso.SelectedValue) : null,
                                                   usuario,
                                                   dtIniConvertido,
                                                   dtFimConvertido,
                                                   idProcessoresposta,
                                                   null,
                                                   ufs,
                                                   idEtapa);

                if (consulta != null && consulta.Any())
                {
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }

                lblQuantidadeEncontrada.Text = string.Format("<b>Total de registros encontrados:</b> {0}", consulta.Count);

                dgRelatorio.DataSource = consulta;
                Session.Add("dsRelatorio", consulta);

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }




        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTOHistoricoProcessos>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTOHistoricoProcessos>)Session["dsRelatorio"],
                                                               dgRelatorio,
                                                               e.SortExpression,
                                                               e.SortDirection,
                                                               "dsRelatorio");
        }

        protected void lkbInfo_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            Response.Redirect("Detalhes.aspx?demanda=" + id);

        }

        protected void cbxProcesso_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado;

            if (int.TryParse(cbxProcesso.SelectedValue, out idSelecionado))
            {
                var etapas = new ManterEtapa().ObterPorProcessoId(idSelecionado);
                WebFormHelper.PreencherLista(etapas, cbxDemanda, false, true);
            }
        }
    }
}