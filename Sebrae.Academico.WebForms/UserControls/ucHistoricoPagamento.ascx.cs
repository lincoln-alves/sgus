using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucHistoricoPagamento : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region "Eventos Relacionados à visualização dos detalhe do Pagamento"


        public delegate void DetalheDoPagamento(object sender, VisualizarDetalheDoPagamentoEventArgs e);
        public event DetalheDoPagamento VisualizouDetalheDoPagamento;

        #endregion

        public void CarregarInformacoesSobrePagamento()
        {
            try
            {

                if (IdUsuario > 0)
                {

                    Usuario usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);

                    if (usuario != null)
                    {
                        IList<UsuarioPagamento> ListaUsuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoDoUsuario(this.IdUsuario);
                        WebFormHelper.PreencherGrid(ListaUsuarioPagamento, dgvInformacoesDeHistoricoDePagamento);
                    }
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public int IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }

        public int IdUsuarioPagamento
        {
            get
            {
                if (ViewState["ViewStateIdUsuarioPagamento"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuarioPagamento"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuarioPagamento"] = value;
            }

        }

        #region "Eventos do Grid"

        protected void dgvInformacoesDeHistoricoDePagamento_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                UsuarioPagamento usuarioPagamento = (UsuarioPagamento)e.Row.DataItem;

                if (usuarioPagamento != null && usuarioPagamento.ID > 0)
                {

                    LinkButton lkblinkSiteBB = (LinkButton)e.Row.FindControl("lkblinkSiteBB");

                    //Quando INPago = false, ou seja, se o pagamento ainda não foi feito, então exibe o link para o site do Banco do Brasil.
                    if (!usuarioPagamento.PagamentoEfetuado)
                    {
                        if (lkblinkSiteBB != null)
                        {
                            lkblinkSiteBB.CommandArgument = usuarioPagamento.ID.ToString();
                            lkblinkSiteBB.Visible = true;
                        }
                        else
                        {
                            lkblinkSiteBB.Visible = false;
                        }
                    }
                    else
                    {
                        lkblinkSiteBB.Visible = false;
                    }

                    CheckBox chkInPago = (CheckBox)e.Row.FindControl("chkInPago");
                    chkInPago.Checked = false;

                    if (usuarioPagamento.PagamentoConfirmado.HasValue && usuarioPagamento.PagamentoConfirmado.Value)
                    {
                        chkInPago.Checked = true;
                    }

                }

            }

        }

        protected void dgvInformacoesDeHistoricoDePagamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //Editar Histórico de Pagamento
            if (e.CommandName.Equals("editar"))
            {
                try
                {
                    this.TratarEdicaoDeHistoriodePagamento(e);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
            //Acessar site do Banco do Brasil
            else if (e.CommandName.Equals("linksitebb"))
            {
                try
                {
                    this.TratarLinkParaOSiteDoBancoDoBrasil(e);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
        }

        #endregion

        private void TratarLinkParaOSiteDoBancoDoBrasil(GridViewCommandEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TratarEdicaoDeHistoriodePagamento(GridViewCommandEventArgs e)
        {
            int idUsuarioPagamento = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.Equals("editar"))
            {
                try
                {
                    /* Se existir um método para manipular o evento 
                       para visualizar os detalhes do pagamento, segue o fluxo */
                    if (this.VisualizouDetalheDoPagamento != null)
                    {
                        this.IdUsuarioPagamento = idUsuarioPagamento;
                    }
                    // ucInformarPagamento1.IdUsuarioPagamento = idUsuarioPagamento;
                    // UsuarioPagamento usuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoPorID(idUsuarioPagamento);
                    //  ucInformarPagamento1.PrepararTelaParaEdicaoDeInformarPagamento(usuarioPagamento);
                    //   this.pnlUcInformarPagamento.Visible = true;
                    //this.ucInformarPagamento1.Visible = true;
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }

                /* Dispara o evento, informando que o usuário clicou no link para visualizar os detalhes do
                   pagamento */
                VisualizouDetalheDoPagamento(this, new VisualizarDetalheDoPagamentoEventArgs(idUsuarioPagamento));
            }
        }

    }

    public class VisualizarDetalheDoPagamentoEventArgs : EventArgs
    {
        public int IdUsuarioPagamento { get; set; }

        //private UsuarioPagamento usuarioPagamento { get; set; }

        public VisualizarDetalheDoPagamentoEventArgs(int pIdUsuarioPagamento)
        {
            IdUsuarioPagamento = pIdUsuarioPagamento;
        }

        //public InformarPagamentoEventArgs(UsuarioPagamento pUsuarioPagamento)
        //{
        //    usuarioPagamento = pUsuarioPagamento;
        //}
    }
}