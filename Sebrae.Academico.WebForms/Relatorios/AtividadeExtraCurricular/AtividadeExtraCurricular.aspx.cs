using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.AtividadeExtraCurricular
{
    public partial class AtividadeExtraCurricular : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            var relAtivExtraCurricular = new RelatorioAtividadeExtraCurricular();

            DateTime? dataTerIni = null;
            DateTime? dataTerFim = null;
            DateTime? dataCadIni = null;
            DateTime? dataCadFim = null;
            DateTime dataTmp;
            int cargaHoraria = 0;

            // Filtro por período de término
            if (!string.IsNullOrWhiteSpace(txtDataTerIni.Text))
            {
                if (DateTime.TryParse(txtDataTerIni.Text, out dataTmp))
                {
                    dataTerIni = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data incial do período de término é inválida");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDataTerFim.Text))
            {
                if (DateTime.TryParse(txtDataTerFim.Text, out dataTmp))
                {
                    dataTerFim = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final do período de término é inválida");
                    return;
                }
            }

            if (dataTerFim.HasValue && dataTerIni.HasValue && dataTerIni > dataTerFim)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final do período de término deve ser maior que a inicial");
                return;
            }

            // Filtro por data de cadastro ou data de atualização (ainda não temos data de criação)
            if (!string.IsNullOrWhiteSpace(txtDataCadIni.Text))
            {
                if (DateTime.TryParse(txtDataCadIni.Text, out dataTmp))
                {
                    dataCadIni = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data incial do período da data de atualização é inválida");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtDataCadFim.Text))
            {
                if (DateTime.TryParse(txtDataCadFim.Text, out dataTmp))
                {
                    dataCadFim = dataTmp;
                }
                else
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data final do período da data de atualização é inválida");
                    return;
                }
            }

            if (dataCadFim.HasValue && dataCadIni.HasValue && dataCadIni > dataCadFim)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "A data final do período de cadastro deve ser maior que a inicial");
                return;
            }

            if (txtCargaHoraria.Text != "")
            {
                cargaHoraria = int.Parse(txtCargaHoraria.Text);
            }            

            var lstGrid = relAtivExtraCurricular.ConsultarRelatorioAtividadeExtraCurricular(dataTerIni, dataTerFim, dataCadIni, dataCadFim, cargaHoraria);

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

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var url = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID( (int) enumConfiguracaoSistema.EnderecoSGUS ).Registro;

            foreach (var item in (IList<DTORelatorioAtividadeExtraCurricular>) dt)
            {   
                if( item.idFileServer != null)
                {
                    item.linkToFile = url + "/MediaServer.ashx?Identificador=" + item.idFileServer.ToString();
                }
            }

            WebFormHelper.GerarRelatorio("AtividadeExtraCurricular.rptAtividadeExtraCurricular.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<DTORelatorioAtividadeExtraCurricular>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<DTORelatorioAtividadeExtraCurricular>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);

        }

        protected void lkbInfo_Click(object sender, EventArgs e)
        {
            var id = ((LinkButton)sender).CommandArgument;

            Response.Redirect("/MediaServer.ashx?Identificador=" + id);

        }
    }
    
}