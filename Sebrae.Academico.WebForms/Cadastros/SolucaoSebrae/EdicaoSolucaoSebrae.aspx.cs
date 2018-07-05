using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.SolucaoSebrae
{
    public partial class EdicaoSolucaoSebrae : Page
    {
        private ItemTrilha _itemtrilhaEdicao;
        private readonly ManterItemTrilha _manterItemTrilha = new ManterItemTrilha();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    Session["ItemTrilhaEdit"] = int.Parse(Request["Id"]);
                }
                else
                {
                    Session.Remove("ItemTrilhaEdit");
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request["Id"] == null)
                {
                    _itemtrilhaEdicao = ucItemTrilha1.ObterObjetoItemTrilha(null);
                    _manterItemTrilha.IncluirItemTrilha(_itemtrilhaEdicao);
                }
                else
                {
                    _itemtrilhaEdicao = ucItemTrilha1.ObterObjetoItemTrilha(int.Parse(Request["Id"]));
                    _manterItemTrilha.AlterarItemTrilha(_itemtrilhaEdicao);
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarSolucaoSebrae.aspx");

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarSolucaoSebrae.aspx");
        }
    }
}