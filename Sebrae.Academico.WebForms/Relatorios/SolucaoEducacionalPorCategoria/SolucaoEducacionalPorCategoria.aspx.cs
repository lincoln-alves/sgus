using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.SolucaoEducacionalPorCategoria
{
    public partial class SolucaoEducacionalPorCategoria : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) 
                return;
            ucCategorias1.PreencherCategorias(false, null);

            var rel = new RelatorioSolucaoEducacionalPorCategoria();
            ListBoxesUFResponsavel.PreencherItens(rel.ObterUFTodos(), "ID", "Nome");
        }


        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            var ids = ucCategorias1.IdsCategoriasMarcadas.Any()
                ? ucCategorias1.IdsCategoriasMarcadas
                : ucCategorias1.IdsCategoriasExistentes;

            var pUfResponsavel = ListBoxesUFResponsavel.RecuperarIdsSelecionados<int>();

            var consulta = new RelatorioSolucaoEducacionalPorCategoria().Consultar(ids.ToList(), pUfResponsavel.ToList()).ToList();

            if (consulta.Any())
            {
                // Converter os resultados em dados totalizadores.
                var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(consulta,
                            "Total da quantidade de Solução por categoria", "Categoria",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(consulta,
                            "Total da quantidade de separadas por fornecedor", "Fornecedor",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(consulta,
                            "Total da quantidade por forma de aquisição", "FormaAquisicao",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(consulta,
                            "Total da quantidade por ativo (sim) e inativo (não)", "Ativo",
                            enumTotalizacaoRelatorio.Contar)
                    };

                ucTotalizadorRelatorio.PreencherTabela(totalizadores);

                componenteGeracaoRelatorio.Visible = true;
                ucFormatoSaidaRelatorio.Visible = true;
                btnConsultar.CssClass = "btn btn-default mostrarload";
                Filtros.CssClass = "panel-collapse collapse";
            }
            else
            {
                ucTotalizadorRelatorio.LimparTotalizadores();
                componenteGeracaoRelatorio.Visible = false;
                ucFormatoSaidaRelatorio.Visible = false;
            }

            dgRelatorio.DataSource = consulta;
            Session.Add("dsRelatorio", consulta);
            WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTOSolucoesPorCategoria>) Session["dsRelatorio"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTOSolucoesPorCategoria>) Session["dsRelatorio"], dgRelatorio,
                e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnConsultar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("SolucaoEducacionalPorCategoria.rptSolucaoEducacionalPorCategoria.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, totalizadores);
        }
    }
}