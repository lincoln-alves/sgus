using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO;
using Sebrae.Academico.BM.Classes.Moodle.Views;
using Sebrae.Academico.BM.Classes.PortalUC;
using Sebrae.Academico.Dominio.Classes.Views;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class LogAcessosPortal : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {

            DateTime? dataInicio = ParserData(txtDataInicial.Text);
            DateTime? dataFim = ParserData(txtDataFinal.Text);

            if (!DatasEstaoValidas(dataInicio, dataFim))
            {
                return;
            }

            IQueryable<LogAcoesPortal> logAcessoPortalQuery = GerarFiltroDeLogs(dataInicio, dataFim);

            IList<DTORelatorioAcessosPortal> lstGrid = AdicionarUsuariosALista(logAcessoPortalQuery);

            Session.Add("dsRelatorio", lstGrid);

            dgRelatorio.DataSource = lstGrid;
            dgRelatorio.DataBind();
            WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);

            if (lstGrid != null && lstGrid.Count() > 0)
            {
                componenteGeracaoRelatorio.Visible = true;
                ucFormatoSaidaRelatorio.Visible = true;
                Filtros.CssClass = "panel-collapse collapse";
            }
            else
            {
                componenteGeracaoRelatorio.Visible = false;
                ucFormatoSaidaRelatorio.Visible = false;
            }
        }

        private IQueryable<LogAcoesPortal> GerarFiltroDeLogs(DateTime? dataInicio, DateTime? dataFim)
        {
            var logAcessoPortalQuery = new BMLogAcoesPortal().ObterTodos();

            if (dataInicio.HasValue)
            {
                logAcessoPortalQuery = logAcessoPortalQuery
                    .Where(x => x.Datacesso.Date >= dataInicio.Value.Date);
            }

            if (dataFim.HasValue)
            {
                logAcessoPortalQuery = logAcessoPortalQuery
                    .Where(x => x.Datacesso.Date <= dataFim.Value.Date);
            }

            if (luUsuario?.SelectedUser?.ID > 0)
            {
                logAcessoPortalQuery = logAcessoPortalQuery
                    .Where(x => x.ID_Usuario == luUsuario.SelectedUser.ID);
            }

            return logAcessoPortalQuery.OrderBy(x => x.Datacesso);
        }

        private static IList<DTORelatorioAcessosPortal> AdicionarUsuariosALista(IEnumerable<LogAcoesPortal> logAcessoPortalQuery)
        {
            var idsUsuarios = logAcessoPortalQuery
                .Select(x => x.ID_Usuario)
                .Distinct()
                .ToArray();

            var users = new BMUsuario()
                .ObterTodosIQueryable()
                .Where(x => idsUsuarios.Contains(x.ID))
                .ToArray();

            var usuarioSistema = new Dominio.Classes.Usuario() { Nome = "SISTEMA" };
            return logAcessoPortalQuery
                .Select(x => new DTORelatorioAcessosPortal
                {
                    Usuario = users.FirstOrDefault(y => y.ID == x.ID_Usuario) != null ? users.FirstOrDefault(y => y.ID == x.ID_Usuario) : usuarioSistema,
                    Acesso = x.Datacesso,
                    Pagina = x.Url,
                    Acao = x.Acao
                })
                .ToList();
        }

        protected void btnResetar_Click(object sender, EventArgs e)
        {
            Session["dsRelatorio"] = null;
            Context.Response.Redirect(Request.RawUrl);
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var listaComInformacoesDeAcesso = (IList<DTORelatorioAcessosPortal>)Session["dsRelatorio"];
            WebFormHelper.GerarRelatorio("AcessosPortal.rptAcessosPortal.rdlc", listaComInformacoesDeAcesso, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioAcessosPortal>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioAcessosPortal>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);

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
            if (dataInicio.HasValue && dataInicio.HasValue && dataInicio > DateTime.Today)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A Data inicial não deve ser futura");
                return false;
            }

            if (dataFim.HasValue && dataFim.HasValue && dataFim > DateTime.Today)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A Data final não deve ser futura");
                return false;
            }

            if (dataFim.HasValue && dataInicio.HasValue && dataFim < dataInicio)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A Data final deve ser maior que a inicial");
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