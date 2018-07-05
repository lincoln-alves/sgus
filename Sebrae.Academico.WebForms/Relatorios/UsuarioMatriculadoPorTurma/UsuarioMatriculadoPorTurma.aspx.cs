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
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class UsuarioMatriculadoPorTurma : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
                using (var relUserMat = new RelatorioUsuarioMatriculadoPorTurma())
                {
                    WebFormHelper.PreencherLista(relUserMat.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, false, true);

                    ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(relUserMat.ObterSolucaoEducacionalPorFormaAquisicao());

                    ViewState["_Oferta"] = null;
                    ViewState["_Turma"] = null;
                }

                //Task #416 - revisão de regras de visualização das UFs
                var ufs = new ManterUf().ObterTodosUf();
                WebFormHelper.PreencherLista(ufs, cbxUF, true);
            }
        }

        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var relUserMat = new RelatorioUsuarioMatriculadoPorTurma())
            {
                var idFormaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue)
                    ? 0
                    : int.Parse(cbxFormaAquisicao.SelectedValue);

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(relUserMat.ObterSolucaoEducacionalPorFormaAquisicao(idFormaAquisicao));

                ViewState["_Oferta"] = null;
                ViewState["_Turma"] = null;
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relUserMat = new RelatorioUsuarioMatriculadoPorTurma())
            {
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                //Parametros
                //Task #416 - revisão de regras de visualização das UFs
                var formaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue) ? null : (int?)int.Parse(cbxFormaAquisicao.SelectedValue);
                var solucaoEducacional = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) ? null : (int?)int.Parse(txtSolucaoEducacional.Text);
                var oferta = string.IsNullOrWhiteSpace(txtOferta.Text) ? null : (int?)int.Parse(txtOferta.Text);
                var turma = string.IsNullOrWhiteSpace(txtTurma.Text) ? null : (int?)int.Parse(txtTurma.Text);
                var pUf = string.IsNullOrWhiteSpace(cbxUF.SelectedValue) ? null : (int.Parse(cbxUF.SelectedValue) == 0 ? null : (int?)int.Parse(cbxUF.SelectedValue));

                var lstRelatorio = relUserMat.ConsultarUsuarioMatriculadoPorTurma(
                    formaAquisicao,
                    solucaoEducacional,
                    oferta,
                    turma,
                    pUf).ToList();

                if (lstRelatorio.Any())
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>();

                    if(string.IsNullOrWhiteSpace(txtTurma.Text))
                    totalizadores.Add(TotalizadorUtil.GetTotalizador(lstRelatorio,
                        "Total da quantidade de alunos registros", "CPF",
                        enumTotalizacaoRelatorio.Contar, false));

                    totalizadores.AddRange(new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstRelatorio, "Total da quantidade de alunos CPF", "CPF",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lstRelatorio, "Total da quantidade por nível ocupacional",
                            "NivelOcupacional", enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(lstRelatorio, "Total da quantidade por status", "StatusMatricula",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(lstRelatorio, "Total da quantidade de alunos UF", "UF",
                            enumTotalizacaoRelatorio.ContarDistintos)
                    });

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);

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

                dgRelatorio.DataSource = lstRelatorio;
                Session.Add("dsRelatorio", lstRelatorio);
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioUsuarioMatriculadoPorTurma>) Session["dsRelatorio"],
                dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioUsuarioMatriculadoPorTurma>) Session["dsRelatorio"],
                dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("UsuarioMatriculadoPorTurma.UsuarioMatriculadoPorTurma.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }

        protected void txtSolucaoEducacional_OnValueChanged(object sender, EventArgs e)
        {
            using (var relUserMat = new RelatorioUsuarioMatriculadoPorTurma())
            {
                var idSolucaoEducacional =
                    int.Parse(string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) ? "0" : txtSolucaoEducacional.Text);

                ViewState["_Oferta"] =
                    Helpers.Util.ObterListaAutocomplete(relUserMat.ObterOfertaPorSolucaoEducacional(idSolucaoEducacional));
            }
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            using (var relUserMat = new RelatorioUsuarioMatriculadoPorTurma())
            {
                var idOferta = int.Parse(string.IsNullOrWhiteSpace(txtOferta.Text) ? "0" : txtOferta.Text);

                ViewState["_Turma"] = Helpers.Util.ObterListaAutocomplete(relUserMat.ObterTurmaPorOferta(idOferta));
            }
        }
    }
}