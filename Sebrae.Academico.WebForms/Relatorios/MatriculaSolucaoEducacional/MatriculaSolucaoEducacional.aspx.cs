using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.MatriculaSolucaoEducacional
{
    public partial class MatriculaSolucaoEducacional : Page
    {
        protected override void OnInit(EventArgs e)
        {
            ucCategorias.TreeNodeCheckChanged += AtualizarComboSolucaoEducacional;
        }

        public void AtualizarComboSolucaoEducacional(object sender, EventArgs e)
        {
            using (var matSol = new RelatorioMatriculaSolucaoEducacional())
            {
                var categorias = ucCategorias.IdsCategoriasMarcadas.ToList();

                var lista = matSol.ObterSolucaoEducacionalPorFormaAquisicao();

                if (categorias.Any())
                    lista = lista.Where(s => s.CategoriaConteudo != null && categorias.Contains(s.CategoriaConteudo.ID));

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            IList<enumStatusMatricula> lstStatusMatricula;

            using (var matSol = new RelatorioMatriculaSolucaoEducacional())
            {
                WebFormHelper.PreencherLista(matSol.ObterFormaAquisicaoTodos().OrderBy(x => x.Nome).ToList(), cbxFormaAquisicao, true);
                lstStatusMatricula = matSol.ObterStatusMatriculaTodos();

                ucCategorias.PreencherCategorias(false, null, null, true);

                var lista = matSol.ObterSolucaoEducacionalPorFormaAquisicao();

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
                ListBoxesUF.PreencherItens(matSol.ObterUFTodos(), "ID", "Nome");
                ListBoxesUFResponsavel.PreencherItens(matSol.ObterUFTodos(), "ID", "Nome");
                
            }

            cbxStatusMatricula.Items.Add(new ListItem("-- Todos --", "0"));
            var i = 1;

            foreach (var s in lstStatusMatricula)
            {
                cbxStatusMatricula.Items.Add(new ListItem(s.ToString(), i.ToString()));
                i++;
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relMatSol = new RelatorioMatriculaSolucaoEducacional())
            {
                DateTime dtIni, dtFim;
                DateTime? dtIniConvertido = null, dtFimConvertido = null;

                if (DateTime.TryParse(txtDataInicio.Text, out dtIni))
                    dtIniConvertido = dtIni;

                if (DateTime.TryParse(txtDataFim.Text, out dtFim))
                    dtFimConvertido = dtFim;

                //parametros
                //Task #416 - revisão de regras de visualização das UFs
                var formaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue)
                    ? null
                    : (int?)int.Parse(cbxFormaAquisicao.SelectedValue);

                var solucaoEducacional = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text)
                    ? null
                    : (int?)int.Parse(txtSolucaoEducacional.Text);

                var statusMAtricula = string.IsNullOrWhiteSpace(cbxStatusMatricula.SelectedValue)
                    ? null
                    : (int?)int.Parse(cbxStatusMatricula.SelectedValue);

                var ufs = ListBoxesUF.RecuperarIdsSelecionados<int>().ToList();
                var ufsResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>().ToList();

                var categorias = ucCategorias.IdsCategoriasMarcadas.Any()
                        ? string.Join(",", ucCategorias.IdsCategoriasMarcadas)
                        : null;

                var lstConsulta = relMatSol.ConsultarMatriculaSolucaoEducacional(
                    formaAquisicao,
                    solucaoEducacional,
                    statusMAtricula,
                    ufs,
                    dtIniConvertido,
                    dtFimConvertido,
                    categorias,
                    ufsResponsavel);

                if (lstConsulta != null && lstConsulta.Count > 0)
                {
                    var totalizadores = new List<DTOTotalizador>();

                    if (string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
                    {
                        totalizadores.Add(TotalizadorUtil.GetTotalizador(lstConsulta,
                            "Total da quantidade de alunos registros", "CPF",
                            enumTotalizacaoRelatorio.Contar, false));
                    }

                    // Converter os resultados em dados totalizadores.
                    totalizadores.AddRange(new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(lstConsulta, "Total da quantidade de alunos CPF", "CPF",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(lstConsulta, "Total da quantidade por nível ocupacional",
                            "NivelOcupacional", enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(lstConsulta, "Total da quantidade de alunos UF", "UF",
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
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                dgRelatorio.DataSource = lstConsulta;
                Session.Add("dsRelatorio", lstConsulta);
                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }


        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var relMatSol = new RelatorioMatriculaSolucaoEducacional())
            {
                var idFormaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedItem.Value)
                    ? 0
                    : int.Parse(cbxFormaAquisicao.SelectedItem.Value);

                var lista = relMatSol.ObterSolucaoEducacionalPorFormaAquisicao(idFormaAquisicao);

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(lista);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("MatriculaSolucaoEducacional.rptMatriculaSolucaoEducacional.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }


        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            WebFormHelper.PaginarGrid((IList<DTORelatorioMatriculaSolucaoEducacional>)Session["dsRelatorio"],
                dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {

            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioAcesso>)Session["dsRelatorio"],
                dgRelatorio,
                e.SortExpression,
                e.SortDirection,
                "dsRelatorio");
        }
    }
}