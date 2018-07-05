using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.MensagemGuia
{
    public partial class ListarMensagemGuia : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    var manterMensagemGuia = new ManterMensagemGuia();

                    var mensagens = manterMensagemGuia.ObterTodos();

                    if (mensagens.Any())
                    {
                        WebFormHelper.PreencherGrid(mensagens.ToList(), dgvMensagemGuia);
                        pnlMensagemGuia.Visible = true;
                    }
                    else
                    {
                        pnlMensagemGuia.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void dgvMensagemGuia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editar"))
                Response.Redirect("EdicaoMensagemGuia.aspx?Id=" + e.CommandArgument);
        }
    }
}