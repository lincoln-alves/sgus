using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.Solicitacoes
{
    public partial class Solicitacoes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var solicitacoes = new ManterSolicitacaoRelatorio().ObterDoUsuarioLogado().OrderByDescending(x => x.DataSolicitacao).ToList();

                Session["dsRelatorioSolicitacoes"] = solicitacoes;

                dgRelatorio.DataSource = solicitacoes;
                dgRelatorio.DataBind();
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<SolicitacaoRelatorio>)Session["dsRelatorioSolicitacoes"], dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<SolicitacaoRelatorio>)Session["dsRelatorioSolicitacoes"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorioSolicitacoes");
        }

        protected void btnBaixarRelatorio_OnServerClick(object sender, EventArgs e)
        {
            var button = (System.Web.UI.HtmlControls.HtmlButton) sender;

            var solicitacaoId = int.Parse(button.Attributes["data-id"]);

            var relatorio = new ManterSolicitacaoRelatorio().ObterPorID(solicitacaoId);

            if (relatorio != null && relatorio.DataGeracao != null)
            {
                //WebFormHelper.GerarArquivoRelatorio(relatorio.ObterSaidaEnum(), relatorio.Arquivo);
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro na tentativa de baixar o arquivo. Tente novamente.");
        }
    }
}