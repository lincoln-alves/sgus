using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class HistoricoPagamento : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        this.CarregarInformacoesSobrePagamento();
        //    }
        //}

        //private void CarregarInformacoesSobrePagamento()
        //{
        //    try
        //    {

        //        if (Session["UsuarioPagamento"] != null)
        //        {

        //            Usuario usuario = new ManterUsuario().ObterUsuarioPorID((int)Session["UsuarioPagamento"]);

        //            if (usuario != null)
        //            {
        //                IList<UsuarioPagamento> ListaUsuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoDoUsuario(this.IdUsuario);
        //                WebFormHelper.PreencherGrid(ListaUsuarioPagamento, dgvInformacoesDeHistoricoDePagamento);
        //            }
        //        }

        //    }
        //    catch (AcademicoException ex)
        //    {
        //        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
        //    }
        //}

        //public int IdUsuario
        //{
        //    get
        //    {
        //        if (ViewState["ViewStateIdUsuario"] != null)
        //        {
        //            return (int)ViewState["ViewStateIdUsuario"];
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    set
        //    {
        //        ViewState["ViewStateIdUsuario"] = value;
        //    }

        //}

        //#region "Eventos do Grid"

        //protected void dgvInformacoesDeHistoricoDePagamento_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{

        //    if (e.Row.RowType.Equals(DataControlRowType.DataRow))
        //    {

        //        UsuarioPagamento usuarioPagamento = (UsuarioPagamento)e.Row.DataItem;


        //        if (usuarioPagamento != null && usuarioPagamento.ID > 0)
        //        {

        //            LinkButton lkblinkSiteBB = (LinkButton)e.Row.FindControl("lkblinkSiteBB");

        //            //Quando INPago = false, ou seja, se o pagamento ainda não foi feito, então exibe o link para o site do Banco do Brasil.
        //            if (!usuarioPagamento.INPago)
        //            {
        //                if (lkblinkSiteBB != null)
        //                {
        //                    lkblinkSiteBB.CommandArgument = usuarioPagamento.ID.ToString();
        //                    lkblinkSiteBB.Visible = true;
        //                }
        //                else
        //                {
        //                    lkblinkSiteBB.Visible = false;
        //                }
        //            }
        //            else
        //            {
        //                lkblinkSiteBB.Visible = false;
        //            }
        //        }

        //    }

        //}

        //protected void dgvInformacoesDeHistoricoDePagamento_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //    //Editar Histórico de Pagamento
        //    if (e.CommandName.Equals("editar"))
        //    {
        //        try
        //        {
        //            this.TratarEdicaoDeHistoriodePagamento(e);
        //        }
        //        catch (AcademicoException ex)
        //        {
        //            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
        //            return;
        //        }

        //    }
        //    //Acessar site do Banco do Brasil
        //    else if (e.CommandName.Equals("linksitebb"))
        //    {
        //        try
        //        {
        //            this.TratarLinkParaOSiteDoBancoDoBrasil(e);
        //        }
        //        catch (AcademicoException ex)
        //        {
        //            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
        //            return;
        //        }
        //    }
        //}

        //#endregion

        //private void TratarLinkParaOSiteDoBancoDoBrasil(GridViewCommandEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //private void TratarEdicaoDeHistoriodePagamento(GridViewCommandEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}


    }
}