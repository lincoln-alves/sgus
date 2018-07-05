using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class RelatorioUsuarioMatriculadoPrograma : Page
    {
        ManterUsuario _manterUsuario = new ManterUsuario();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                using (BP.Relatorios.RelatorioUsuarioMatriculadoPrograma relUsuaMatricula = new BP.Relatorios.RelatorioUsuarioMatriculadoPrograma())
                {
                    WebFormHelper.PreencherLista(relUsuaMatricula.ObterProgramaTodos(), cbxPrograma, true, false);
                    WebFormHelper.PreencherListaStatusMatricula(cbxStatusMatricula, true, false);
                }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relUsuaMatricula = new BP.Relatorios.RelatorioUsuarioMatriculadoPrograma())
            {
                var lstGrid =
                    relUsuaMatricula.ConsultarUsuarioMatriculaPrograma(
                        string.IsNullOrWhiteSpace(cbxPrograma.SelectedValue) ? 0 : int.Parse(cbxPrograma.SelectedValue),
                        string.IsNullOrWhiteSpace(cbxStatusMatricula.SelectedValue)
                            ? 0
                            : int.Parse(cbxStatusMatricula.SelectedValue));

                // Converter os resultados em dados totalizadores.
                var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade de alunos registros", "CPF",
                            enumTotalizacaoRelatorio.Contar, false),
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade de alunos CPF", "CPF",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade por nível ocupacional",
                            "NivelOcupacional", enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade por status", "StatusMatricula",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(lstGrid, "Total da quantidade de alunos UF", "UF",
                            enumTotalizacaoRelatorio.ContarDistintos)
                    };

                ucTotalizadorRelatorio.PreencherTabela(totalizadores);


                if (lstGrid != null && lstGrid.Any())
                {
                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                    componenteGeracaoRelatorio.Visible = false;
                }

                Session.Add("dsRelatorio", lstGrid);

                dgRelatorio.DataSource = lstGrid;
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioUsuarioMatriculadoPrograma>) Session["dsRelatorio"],
                dgRelatorio,
                e.SortExpression,
                e.SortDirection,
                "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioUsuarioMatriculadoPrograma>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("UsuarioMatriculadoPrograma.rptRelatorioUsuarioMatriculadoPrograma.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }
    }
}