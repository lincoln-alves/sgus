using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO;
using System.Text;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class RelatorioPaginasAcessos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            var LogAcessoPagina = new BMLogAcessoPagina();

            DateTime? dataInicio = ParserData(txtDataInicial.Text);
            DateTime? dataFim = ParserData(txtDataFinal.Text);

            if (!DatasEstaoValidas(dataInicio, dataFim))
            {
                return;
            }

            var logAcessoPaginaQuery = LogAcessoPagina
                .ObterTodosQuerable()
                .Where(x => x.Pagina != null);

            if (dataInicio.HasValue)
            {
                logAcessoPaginaQuery = logAcessoPaginaQuery
                    .Where(x => x.DTSolicitacao.Date >= dataInicio.Value.Date);
            }

            if (dataFim.HasValue)
            {
                logAcessoPaginaQuery = logAcessoPaginaQuery
                    .Where(x => x.DTSolicitacao.Date <= dataFim.Value.Date);
            }

            if (luUsuario?.SelectedUser?.ID > 0)
            {
                logAcessoPaginaQuery = logAcessoPaginaQuery
                    .Where(x => x.IDUsuario.ID == luUsuario.SelectedUser.ID);
            }

            var baseUrl = Context.Request.Url;

            var lstGrid = logAcessoPaginaQuery.OrderBy(x => x.DTSolicitacao).ToList()
                .Select(x => new DTORelatorioAcessosPaginas
                {
                    Nome = x.IDUsuario.Nome,
                    Cpf = x.IDUsuario.CPF,
                    Acesso = x.DTSolicitacao,
                    Pagina = FormatUrl(baseUrl, x),
                    Acao = x.Acao.GetDescription()
                })
                .ToList();

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
        private static string FormatUrl (Uri uri, Dominio.Classes.LogAcessoPagina log)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(string.Format("{0}://{1}{2}", uri.Scheme, uri.Authority, log.Pagina.CaminhoRelativo));

            if(!string.IsNullOrEmpty(log.QueryString))
            {
                strb.Append("?" + log.QueryString);
            }

            return strb.ToString();
                       
        }
        /// <summary>
        /// Faz o agrupamento das informações de acesso, por data de acesso.
        /// </summary>
        /// <param name="informacoesDeAcesso">Informações de acesso, do usuário</param>
        private IList<DTORelatorioAcessosPaginas> AgruparInformacoesParaOGrafico(IList<DTORelatorioAcessosPaginas> informacoesDeAcesso)
        {
            //Faz o agrupamento dos dados do relatório, em memória.
            var dadosDeAcessoAgrupadosPorData =
                    (from i in informacoesDeAcesso
                     group i by i.Acesso.Date
                    into g
                     select new DTORelatorioAcessosPaginas { Acesso = g.Key, Quantidade = g.Count() })
                    .OrderBy(x => x.Acesso).ToList();

            return dadosDeAcessoAgrupadosPorData;
        }


        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var listaComInformacoesDeAcesso = (IList<DTORelatorioAcessosPaginas>)Session["dsRelatorio"];
            WebFormHelper.GerarRelatorio("AcessosPaginas.rptAcessosPaginas.rdlc", listaComInformacoesDeAcesso, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioAcessosPaginas>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioAcessosPaginas>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);

        }
        private DateTime? ParserData(string data)
        {
            DateTime dataTmp;

            if (!string.IsNullOrWhiteSpace(data))
            {
                if (DateTime.TryParse(data, out dataTmp))
                {
                    return dataTmp;
                }

            }

            return null;
        }

        private bool DatasEstaoValidas(DateTime? dataInicio, DateTime? dataFim)
        {

            if (dataFim.HasValue && dataInicio.HasValue && dataFim < dataInicio)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final deve ser maior que a inicial");
                return false;
            }

            if (dataFim > DateTime.Today)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não é possível pesquisar no período indicado");
                return false;
            }

            return true;
        }
    }
}