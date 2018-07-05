using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.UsuarioPagante
{
    public partial class UsuarioPagante : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            WebFormHelper.PreencherLista(new ManterUf().ObterTodosUf(), cbxUF, true);
            WebFormHelper.PreencherLista(ObterNiveisOcupacionaisPagantes(), cbxNivelOcupacional, true);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var bp = new RelatorioUsuarioPagante())
            {
                var pNome = string.IsNullOrWhiteSpace(txtNome.Text) ? null : txtNome.Text.Trim();

                var pCpf = string.IsNullOrWhiteSpace(txtCPF.Text) ? null : txtCPF.Text.Replace("-", "").Replace(".", "");

                var pUf = string.IsNullOrWhiteSpace(cbxUF.SelectedValue)
                    ? null
                    : (int.Parse(cbxUF.SelectedValue) == 0 ? null : (int?)int.Parse(cbxUF.SelectedValue));

                var pNivelOcupacional =
                    string.IsNullOrWhiteSpace(cbxNivelOcupacional.SelectedValue)
                        ? null
                        : int.Parse(cbxNivelOcupacional.SelectedValue) == 0 ? null : (int?)int.Parse(cbxNivelOcupacional.SelectedValue);

                /*  Valores de acordo com a procedure:
                 *  0: Todos
                 *  1: Pagantes (Padrão)
                 *  2: Não Pagantes
                 */
                var pTipo = string.IsNullOrWhiteSpace(rblPagante.SelectedValue)
                    ? null
                    : int.Parse(rblPagante.SelectedValue) == 0 ? null : (int?)int.Parse(rblPagante.SelectedValue);

                var consulta = bp.ConsultarRelatorio(pNome, pCpf, pUf, pNivelOcupacional, pTipo).ToList();

                if (consulta.Any())
                {
                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        new DTOTotalizador
                        {
                            Descricao = "Total da quantidade de alunos pagos",
                            Dado = (
                                int.Parse(
                                    TotalizadorUtil.GetTotalizador(consulta, "", "CPF",
                                        enumTotalizacaoRelatorio.ContarDistintosPorValor, false, "Pago", true)
                                        .Dado.ToString())
                                +
                                int.Parse(
                                    TotalizadorUtil.GetTotalizador(consulta, "", "CPF",
                                        enumTotalizacaoRelatorio.ContarDistintosPorValor, false, "PagamentoInformado", true)
                                        .Dado.ToString())
                                +
                                int.Parse(
                                    TotalizadorUtil.GetTotalizador(consulta, "", "CPF",
                                        enumTotalizacaoRelatorio.ContarDistintosPorValor, false, "PagamentoConfirmado", true)
                                        .Dado.ToString())
                                )
                        },
                        new DTOTotalizador
                        {
                            Descricao = "Total da quantidade de alunos não pagos",
                            Dado = (
                                int.Parse(
                                    TotalizadorUtil.GetTotalizador(consulta, "", "CPF",
                                        enumTotalizacaoRelatorio.ContarDistintosPorValor, false, "Pago", false)
                                        .Dado.ToString())
                                +
                                int.Parse(
                                    TotalizadorUtil.GetTotalizador(consulta, "", "CPF",
                                        enumTotalizacaoRelatorio.ContarDistintosPorValor, false, "PagamentoInformado", false)
                                        .Dado.ToString())
                                +
                                int.Parse(
                                    TotalizadorUtil.GetTotalizador(consulta, "", "CPF",
                                        enumTotalizacaoRelatorio.ContarDistintosPorValor, false, "PagamentoConfirmado", false)
                                        .Dado.ToString())
                                )
                        }
                        ,
                        TotalizadorUtil.GetTotalizador(consulta, "Total da quantidade por nível ocupacional",
                            "NivelOcupacional", enumTotalizacaoRelatorio.ContarDistintos),
                        TotalizadorUtil.GetTotalizador(consulta, "Total da quantidade por UF",
                            "UF", enumTotalizacaoRelatorio.ContarDistintos)
                    };

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);

                    Session.Add("dsRelatorio", consulta);
                    WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);

                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                dgRelatorio.DataSource = consulta;
                dgRelatorio.DataBind();
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];
            WebFormHelper.GerarRelatorio("UsuarioPagante.rptUsuarioPagante.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }



        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioUsuarioPagante>)Session["dsRelatorio"],
                dgRelatorio,
                e.SortExpression,
                e.SortDirection,
                "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioUsuarioPagante>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        private IList<NivelOcupacional> ObterNiveisOcupacionaisPagantes()
        {
            string[] niveisPagantes = { "ALI", "AOE", "CREDENCIADO", "PARCEIRO" };

            return new ManterNivelOcupacional().ObterTodosNivelOcupacional()
                .Where(n => niveisPagantes.Contains(n.Nome.ToUpper())).ToList();
        }

        protected void rblPagante_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pagoCheckBox = chkListaCamposVisiveis.Items.FindByValue("DescricaoPago");
            var pagamentoInformadoCheckBox = chkListaCamposVisiveis.Items.FindByValue("DescricaoPagamentoInformado");
            var pagamentoConfirmadoCheckBox = chkListaCamposVisiveis.Items.FindByValue("DescricaoPagamentoConfirmado");

            if (rblPagante.SelectedValue == "2")
            {
                pagoCheckBox.Selected = false;
                pagoCheckBox.Enabled = false;

                pagamentoInformadoCheckBox.Selected = false;
                pagamentoInformadoCheckBox.Enabled = false;

                pagamentoConfirmadoCheckBox.Selected = false;
                pagamentoConfirmadoCheckBox.Enabled = false;
            }
            else
            {
                pagoCheckBox.Selected = true;
                pagoCheckBox.Enabled = true;

                pagamentoInformadoCheckBox.Selected = true;
                pagamentoInformadoCheckBox.Enabled = true;

                pagamentoConfirmadoCheckBox.Selected = true;
                pagamentoConfirmadoCheckBox.Enabled = true;
            }
        }
    }
}