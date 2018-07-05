using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.UsuarioMatriculadoSEFormaAquisicao
{
    public partial class RelatorioUsuarioMatriculadoSEFormaAquisicao : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            using (var relHist = new BP.Relatorios.RelatorioUsuarioMatriculadoSEFormaAquisicao())
            {
                var formasDeAquisicao = Session["formasDeAquisicao"] == null
                    ? relHist.ObterFormaAquisicaoTodos()
                    : (IList<FormaAquisicao>)Session["formasDeAquisicao"];

                WebFormHelper.PreencherLista(formasDeAquisicao, cbxPrograma, true);
                WebFormHelper.PreencherListaStatusMatricula(cbxStatusMatricula, true, false);

                ListBoxesUFResponsavel.PreencherItens(relHist.ObterUFTodos(), "ID", "Nome");

                txtDataInicioTurma.Text = string.Empty;
                txtDataFinalTurma.Text = string.Empty;
            }

            //Task #416 - revisão de regras de visualização das UFs
            var ufs = new ManterUf().ObterTodosUf();
            WebFormHelper.PreencherLista(ufs, cbxUF);

            var tiposFormaAquisicao =
                Enum.GetValues(typeof(enumTipoFormaAquisicao))
                    .Cast<enumTipoFormaAquisicao>()
                    .Select(t => new { nome = t.GetDescription(), valor = (int)t }).ToList();

            WebFormHelper.PreencherListaCustomizado(tiposFormaAquisicao, cbxFormaAquisicao, "valor", "nome", true);
        }

        protected void dsUsuarioMatriculadoSEFormaAquisicao_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioUsuarioMatriculadoPrograma>)Session["dsRelatorio"],
                dgRelatorio, e.NewPageIndex);
        }

        protected void dsUsuarioMatriculadoSEFormaAquisicao_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relUsuaMatricula = new BP.Relatorios.RelatorioUsuarioMatriculadoSEFormaAquisicao())
            {
                DateTime? dataInicio = null;
                DateTime? dataFim = null;
                DateTime dataTmp;

                if (!string.IsNullOrWhiteSpace(txtDataInicioTurma.Text))
                {
                    if (DateTime.TryParse(txtDataInicioTurma.Text, out dataTmp))
                    {
                        dataInicio = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial inválida");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDataFinalTurma.Text))
                {
                    if (DateTime.TryParse(txtDataFinalTurma.Text, out dataTmp))
                    {
                        dataFim = dataTmp;
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final inválida");
                        return;
                    }
                }

                if (dataFim.HasValue && dataInicio.HasValue && dataFim < dataInicio)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final deve ser maior que a inicial");
                    return;
                }

                //Task #416 - revisão de regras de visualização das UFs
                var pUf = string.IsNullOrWhiteSpace(cbxUF.SelectedValue) ? null : (int.Parse(cbxUF.SelectedValue) == 0 ? null : (int?)int.Parse(cbxUF.SelectedValue));

                List<int> idsFormaAquisicao = new List<int>();
                
                if (cbxPrograma.SelectedItem.Value == "0")
                {
                    foreach (ListItem item in cbxPrograma.Items)
                        if (!string.IsNullOrEmpty(item.Value)) idsFormaAquisicao.Add(int.Parse(item.Value));
                }
                else
                {
                    idsFormaAquisicao.Add(int.Parse(cbxPrograma.SelectedItem.Value));
                }

                var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

                var lstGrid = relUsuaMatricula.ConsultarRelatorioUsuarioMatriculadoSeFormaAquisicao(
                    idsFormaAquisicao,
                    string.IsNullOrWhiteSpace(cbxStatusMatricula.SelectedValue)
                        ? null
                        : (int?)cbxStatusMatricula.SelectedIndex,
                    dataInicio,
                    dataFim,
                    pUf,
                    pUfResponsavel)
                    .OrderBy(x => x.FormaAquisicao)
                    .ThenBy(x => x.SolucaoEducacional)
                    .ThenBy(x => x.Oferta)
                    .ToList();

                if (lstGrid.Any())
                {
                    componenteGeracaoRelatorio.Visible =
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    componenteGeracaoRelatorio.Visible =
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                Session.Add("dsRelatorio", lstGrid);

                dgRelatorio.DataSource = lstGrid;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio(
                "UsuarioMatriculadoSEFormaAquisicao.rptRelatorioUsuarioMatriculadoSEFormaAquisicao.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioUsuarioMatriculadoSEFormaAquisicao>)Session["dsRelatorio"],
                dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid(
                (IList<DTORelatorioUsuarioMatriculadoSEFormaAquisicao>)Session["dsRelatorio"],
                dgRelatorio,
                e.SortExpression,
                e.SortDirection,
                "dsRelatorio");
        }

        protected void cbxFormaAquisicao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            IList<FormaAquisicao> formasAquisicao;
            int formaAquisicao = 0;

            using (var relHist = new BP.Relatorios.RelatorioUsuarioMatriculadoSEFormaAquisicao())
            {
                formasAquisicao = Session["formasDeAquisicao"] == null
                    ? relHist.ObterFormaAquisicaoTodos()
                    : (IList<FormaAquisicao>)Session["formasDeAquisicao"];
            }

            if (int.TryParse(cbxFormaAquisicao.SelectedValue, out formaAquisicao))
            {
                switch (formaAquisicao)
                {
                    case (int)enumTipoFormaAquisicao.SolucaoEducacional:
                        formasAquisicao =
                            formasAquisicao.Where(
                                x => (int)x.TipoFormaDeAquisicao == (int)enumTipoFormaAquisicao.SolucaoEducacional)
                                .ToList();
                        break;
                    case (int)enumTipoFormaAquisicao.Trilha:
                        formasAquisicao =
                            formasAquisicao.Where(
                                x => (int)x.TipoFormaDeAquisicao == (int)enumTipoFormaAquisicao.Trilha).ToList();
                        break;
                }
                WebFormHelper.PreencherLista(formasAquisicao, cbxPrograma, true);
            }
            else
            {
                // Variável adicionada na sessão no pageload.
                WebFormHelper.PreencherLista(formasAquisicao, cbxPrograma, true);
            }
        }
    }
}