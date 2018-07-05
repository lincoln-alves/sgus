using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class RelatorioAcessos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherListas();
                txtDataFinal.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtDataFinal.Text = DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy");
            }
        }

        private void PreencherListas()
        {
            WebFormHelper.PreencherLista(new ManterUf().ObterTodosUf(), cbxUF, true, false);
            WebFormHelper.PreencherLista(new ManterNivelOcupacional().ObterTodosNivelOcupacional(), cbxNivelOcupacional, true, false);
            WebFormHelper.PreencherLista(new ManterPerfil().ObterTodosPerfis(), cbxPefil, true, false);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            var relDadosPessoais = new RelatorioAcesso();

            DateTime? dataInicio = null;
            DateTime? dataFim = null;
            DateTime dataTmp;

            if (!string.IsNullOrWhiteSpace(txtDataInicial.Text))
            {
                if (DateTime.TryParse(txtDataInicial.Text, out dataTmp))
                {
                    dataInicio = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial inválida");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDataFinal.Text))
            {
                if (DateTime.TryParse(txtDataFinal.Text, out dataTmp))
                {
                    dataFim = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data Final inválida");
                    return;
                }
            }

            if (dataFim.HasValue && dataInicio.HasValue && dataFim < dataInicio)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final deve ser maior que a inicial");
                return;
            }

            var pIdSelectedUser = luUsuario.SelectedUser == null
                ? null
                : (luUsuario.SelectedUser.ID == 0 ? null : (int?) luUsuario.SelectedUser.ID);

            var pIdUf = string.IsNullOrWhiteSpace(cbxUF.SelectedValue)
                ? null
                : int.Parse(cbxUF.SelectedValue) == 0 ? null : (int?) int.Parse(cbxUF.SelectedValue);

            var pIdNivelOcupacional = string.IsNullOrWhiteSpace(cbxNivelOcupacional.SelectedValue)
                ? null
                : (int.Parse(cbxNivelOcupacional.SelectedValue) == 0 ? null : (int?) int.Parse(cbxNivelOcupacional.SelectedValue));

            var pIdPerfil = string.IsNullOrWhiteSpace(cbxPefil.SelectedValue)
                ? null
                : (int.Parse(cbxPefil.SelectedValue) == 0 ? null : (int?)int.Parse(cbxPefil.SelectedValue));

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var lstGrid = relDadosPessoais.ConsultarRelatorioAcesso(pIdSelectedUser, pIdUf, pIdNivelOcupacional, pIdPerfil, dataInicio, dataFim);

            Session.Add("dsRelatorio", lstGrid);

            dgRelatorio.DataSource = lstGrid;
            dgRelatorio.DataBind();
            WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);

            if (lstGrid != null && lstGrid.Count > 0)
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
        }

        /// <summary>
        /// Faz o agrupamento das informações de acesso, por data de acesso.
        /// </summary>
        /// <param name="informacoesDeAcesso">Informações de acesso, do usuário</param>
        private IList<DTORelatorioAcesso> AgruparInformacoesParaOGrafico(IList<DTORelatorioAcesso> informacoesDeAcesso)
        {
            //Faz o agrupamento dos dados do relatório, em memória.
            var dadosDeAcessoAgrupadosPorData =
                    (from i in informacoesDeAcesso
                    group i by i.Acesso.Date
                    into g
                    select new DTORelatorioAcesso {Acesso = g.Key, Quantidade = g.Count()})
                    .OrderBy(x => x.Acesso).ToList();

            return dadosDeAcessoAgrupadosPorData;
        }


        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var listaComInformacoesDeAcesso = (IList<DTORelatorioAcesso>)Session["dsRelatorio"];
            var informacoesAgrupadasPorDataDeAcesso = this.AgruparInformacoesParaOGrafico(listaComInformacoesDeAcesso);
            var listaSubRelatorios = new List<string> {"Acessos.rptAcessoAgrupadoPorDataAcesso.rdlc"};
            WebFormHelper.GerarRelatorioComGrafico("Acessos.rptAcesso.rdlc", listaSubRelatorios, listaComInformacoesDeAcesso, informacoesAgrupadasPorDataDeAcesso, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioAcesso>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioAcesso>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);

        }
    }
}