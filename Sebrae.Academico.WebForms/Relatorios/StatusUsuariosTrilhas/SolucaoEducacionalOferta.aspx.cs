using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class SolucaoEducacionalOferta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            using (RelatorioSolucaoEducacionalOferta relSEO = new RelatorioSolucaoEducacionalOferta()) 
            {

                WebFormHelper.PreencherLista(relSEO.ObterTipoOfertaTodos(), cbxTipoOferta, true, false);
                WebFormHelper.PreencherLista(relSEO.ObterFormaAquisicaoTodos(), cbxFormaAquisicao, true, false);
            }
        }

        protected void cbxFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (RelatorioSolucaoEducacionalOferta relSEO = new RelatorioSolucaoEducacionalOferta())
            {
                int idForma = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue) ? 0 : int.Parse(cbxFormaAquisicao.SelectedValue);
                int ufPermitida = new BMUsuario().ObterUFLogadoSeGestor();
                var lista = relSEO.ObterSolucaoEducacionalPorFormaAquisicao(idForma);
                if (ufPermitida > 0)
                {
                    lista = lista.Where(x => x.ListaPermissao.Any(p => p.Uf != null || p.Uf.ID == ufPermitida)).ToList();
                }

                WebFormHelper.PreencherLista(lista,cbxSolucaoEducacional,true,false);
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            using (RelatorioSolucaoEducacionalOferta relSO = new RelatorioSolucaoEducacionalOferta())
            {

                int IdTipoOferta = string.IsNullOrWhiteSpace(cbxTipoOferta.SelectedValue) ? 0 : int.Parse(cbxTipoOferta.SelectedValue);
                int IdFormaAquisicao = string.IsNullOrWhiteSpace(cbxFormaAquisicao.SelectedValue) ? 0 : int.Parse(cbxFormaAquisicao.SelectedValue);
                int IdSolucaoEducacional = string.IsNullOrWhiteSpace(cbxSolucaoEducacional.SelectedValue) ? 0 : int.Parse(cbxSolucaoEducacional.SelectedValue);

                IList<DTOSolucaoEducacionalOferta> lstRelatorio = relSO.ConsultarSolucaoEducacionalOferta(IdFormaAquisicao, IdTipoOferta, IdSolucaoEducacional);

                Session.Add("dsRelatorio", lstRelatorio);

                if (lstRelatorio != null && lstRelatorio.Count > 0)
                {
                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnConsultar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }


                dgRelatorio.DataSource = lstRelatorio;

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid<DTOSolucaoEducacionalOferta>((IList<DTOSolucaoEducacionalOferta>)Session["dsRelatorio"], dgRelatorio,e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid<DTOSolucaoEducacionalOferta>((IList<DTOSolucaoEducacionalOferta>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnConsultar_Click(null, null);
            
            var dt = Session["dsRelatorio"];

            WebFormHelper.GerarRelatorio("SolucaoEducacionalOferta.rptSolucaoEducacionalOferta.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }

        
    }
}