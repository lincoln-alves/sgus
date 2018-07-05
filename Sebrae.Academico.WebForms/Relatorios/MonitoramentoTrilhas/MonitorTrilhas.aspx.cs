using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.MonitoramentoTrilhas
{
    public partial class MonitorTrilhas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variável para impedir execuções desecessárias da busca de SEs com filtro de categoria.
            ViewState["buscouCategorias"] = true;

            if (IsPostBack) return;

            using (var rel = new RelatorioMonitoramentotrilhas())
            {
                var ls = rel.ObterTrilhasComParticipacao();

                if (ls != null)
                    WebFormHelper.PreencherLista(ls.Select(x => new { x.ID, x.Nome }).ToList(), cbxTrilha, true);

                WebFormHelper.PreencherLista(
                    new List<object> { new { ID = 0, Nome = "-- TSelecione um Nível da Trilha --" } }, cbxMonitor);
            }
        }

        protected void cbxTipoParticipacao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int numTipoParticipacao;

            int.TryParse(cbxTipoParticipacao.SelectedValue, out numTipoParticipacao);

            if (numTipoParticipacao != 0)
            {
                for (var i = 0; i < chkListaCamposVisiveis.Items.Count; i++)
                {
                    chkListaCamposVisiveis.Items[i].Enabled = false;
                    chkListaCamposVisiveis.Items[i].Selected = false;
                }
            }

            switch (numTipoParticipacao)
            {
                case 1:
                    {
                        var ls = new List<int> { 5, 6, 7, 8, 9, 10 };
                        foreach (var i in ls)
                        {
                            chkListaCamposVisiveis.Items[i].Enabled = true;
                            chkListaCamposVisiveis.Items[i].Selected = true;
                        }
                        break;
                    }
                case 2:
                    {
                        var ls = new List<int> { 11, 12, 13, 14, 15, 16 };
                        foreach (var i in ls)
                        {
                            chkListaCamposVisiveis.Items[i].Enabled = true;
                            chkListaCamposVisiveis.Items[i].Selected = true;
                        }
                        break;
                    }
                case 3:
                    {
                        var ls = new List<int> { 17, 18, 19, 20, 21, 22, };
                        foreach (var i in ls)
                        {
                            chkListaCamposVisiveis.Items[i].Enabled = true;
                            chkListaCamposVisiveis.Items[i].Selected = true;
                        }
                        break;
                    }
                default:
                    {
                        for (var i = 0; i < chkListaCamposVisiveis.Items.Count; i++)
                        {
                            chkListaCamposVisiveis.Items[i].Enabled = true;
                            chkListaCamposVisiveis.Items[i].Selected = true;
                        }
                        break;
                    }
            }
        }

        protected void cbxTrilha_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var idTrilha = 0;
            if (!int.TryParse(cbxTrilha.SelectedValue, out idTrilha)) return;
            var manterTrilha = new ManterTrilha();
            var trilha = manterTrilha.ObterTrilhaPorId(idTrilha);
            if (trilha == null) return;
            WebFormHelper.PreencherLista(trilha.ListaTrilhaNivel.Where(p => p.ListaItemTrilha.Any()).ToList(),
                cbxNivelTrilha, true);
        }

        protected void cbxNivelTrilha_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var idTrilha = 0;
            if (!int.TryParse(cbxTrilha.SelectedValue, out idTrilha)) return;
            var idTrilhaNivel = 0;
            if (!int.TryParse(cbxNivelTrilha.SelectedValue, out idTrilhaNivel)) return;

            using (var rel = new RelatorioMonitoramentotrilhas())
            {
                WebFormHelper.PreencherLista(rel.ObterMonitores(idTrilha, idTrilhaNivel), cbxMonitor, true);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SelecionarPagina(e.NewPageIndex);
        }

        /// <summary>
        /// Filtrar os resultados do GridView por página e executa contagem de acessos somentes nos itens sendo exibidos na página.
        /// </summary>
        /// <param name="pageIndex">Índice da página</param>
        /// <returns></returns>
        private List<DTOMonitoramentoTrilhas> SelecionarPagina(int? pageIndex = null)
        {
            if (pageIndex == null)
                pageIndex = dgRelatorio.PageIndex;

            var query = (IList<DTOMonitoramentoTrilhas>)Session["dsRelatorio"];

            if (pageIndex != 0)
                query = query.Skip(pageIndex.Value * dgRelatorio.PageSize).ToList();

            var relatorio = query.Take(100).ToList();

            WebFormHelper.PaginarGrid(relatorio, dgRelatorio, pageIndex.Value);

            return relatorio;
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            var query = (IList<DTOMonitoramentoTrilhas>)Session["dsRelatorio"];

            WebFormHelper.OrdenarListaGrid(query, dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio", false);

            SelecionarPagina();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var rel = new RelatorioMonitoramentotrilhas())
            {
                DateTime? dataInicio;
                DateTime? dataFim;

                DateTime dataTmp;

                if (string.IsNullOrWhiteSpace(txtDataInicio.Text))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial obrigatória");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDataFinal.Text))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final obrigatória");
                    return;
                }

                if (DateTime.TryParse(txtDataInicio.Text, out dataTmp))
                {
                    dataInicio = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data inicial inválida");
                    return;
                }

                if (DateTime.TryParse(txtDataFinal.Text, out dataTmp))
                {
                    dataFim = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final inválida");
                    return;
                }

                if (dataFim.Value.Date < dataInicio.Value.Date)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final deve ser maior que a inicial");
                    return;
                }

                int idTrilha;
                int idTrilhaNivel;
                int idMonitor;
                int tipoParticipacao;

                int.TryParse(cbxTrilha.SelectedValue, out idTrilha);
                int.TryParse(cbxNivelTrilha.SelectedValue, out idTrilhaNivel);
                int.TryParse(cbxMonitor.SelectedValue, out idMonitor);
                int.TryParse(cbxTipoParticipacao.SelectedValue, out tipoParticipacao);

                var lst = rel.ConsultarMonitoramentoTrilhas(idTrilha, idTrilhaNivel, idMonitor, tipoParticipacao,
                    dataInicio.Value, dataFim.Value);

                var totalizadores = new List<DTOTotalizador>();

                if (lst.Any())
                {
                    // Converter os resultados em dados totalizadores.
                    totalizadores.AddRange(new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lst, "Total da quantidade de Monitores", "CPF",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lst, "Total da quantidade de Trilhas", "IdTrilha",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lst, "Total da quantidade de Níveis da Trilha", "IdTrilhaNivel",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                    });

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                }

                // Insere a lista completa na sessão para não levar somente os resultados da página atual para a impressão.
                Session["dsRelatorio"] = lst;

                var resultado = SelecionarPagina();

                if (resultado.Any())
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

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_OnClick(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizador = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("MonitoramentoTrilhas.rptMonitoramentoTrilhas.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizador);
        }
    }
}