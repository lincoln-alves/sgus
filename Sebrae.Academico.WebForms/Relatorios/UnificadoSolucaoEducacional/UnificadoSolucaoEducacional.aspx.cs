using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.UnificadoSolucaoEducacional
{
    public partial class UnificadoSolucaoEducacional : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (var relDa = new RelatorioUnificadoSolucaoEducacional())
            {
                ListBoxesFormaDeAquisicao.PreencherItens(relDa.ObterFormasDeAquisicao(), "ID", "Nome");
                ListBoxesTiposDeOferta.PreencherItens(relDa.ObterTiposOferta(), "ID", "Nome");
                ListBoxesProgramas.PreencherItens(relDa.ObterProgramas(), "ID", "Nome");
                ucCategorias1.PreencherCategorias();
                ListBoxesPublicosAlvo.PreencherItens(relDa.ObterPublicosAlvo(), "ID", "Nome");
                ListBoxesNivelOcupacional.PreencherItens(relDa.ObterNiveisOcupacionais(), "ID", "Nome");
                ListBoxesPerfis.PreencherItens(relDa.ObterPerfis(), "ID", "Nome");
                ListBoxesUF.PreencherItens(relDa.ObterUFS(), "ID", "Nome");
                ListBoxesUFResponsavel.PreencherItens(relDa.ObterUFS(), "ID", "Nome");
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var relatorio = (IList<DTOUnificadoSolucaoEducacional>) Session["dsRelatorio"];

            relatorio.Skip(e.NewPageIndex*100).Take(100).ToList();

            WebFormHelper.PaginarGrid(relatorio, dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTODesempenhoAcademico>) Session["dsRelatorio"], dgRelatorio,
                e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relDa = new RelatorioUnificadoSolucaoEducacional())
            {
                var lstDa = relDa.ConsultarSolucaoEducacional(
                    ListBoxesFormaDeAquisicao.RecuperarIdsSelecionados<int>(),
                    ListBoxesTiposDeOferta.RecuperarIdsSelecionados<int>(),
                    ListBoxesProgramas.RecuperarIdsSelecionados<int>(),
                    ucCategorias1.IdsCategoriasMarcadas,
                    ListBoxesPublicosAlvo.RecuperarIdsSelecionados<int>(),
                    ListBoxesNivelOcupacional.RecuperarIdsSelecionados<int>(),
                    ListBoxesPerfis.RecuperarIdsSelecionados<int>(),
                    ListBoxesUF.RecuperarIdsSelecionados<int>(),
                    ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>());

                if (lstDa.Count > 0)
                {
                    liTotalizador.Text = string.Format("Foram encontrados {0} registros ", lstDa.Count());
                }

                lstDa.Take(100).ToList();

                Session.Add("dsRelatorio", lstDa);

                dgRelatorio.DataSource = lstDa;

                if (lstDa.Count > 0)
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

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("UnificadoSolucaoEducacional.rptUnificadoSolucaoEducacional.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }
    }
}