using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Hierarquia
{
    public partial class ListarHierarquia : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptUfs.DataSource = new ManterUf().ObterTodosUf();
                rptUfs.DataBind();
            }
        }

        protected void rptUfs_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (Uf)e.Item.DataItem;

                var btnUf = (Button)e.Item.FindControl("btnUf");

                btnUf.Text = item.Nome + " - " + item.Sigla;

                btnUf.Attributes.Add("ID_UF", item.ID.ToString());
            }
        }

        protected void btnUf_OnClick(object sender, EventArgs e)
        {
            var ufId = ((Button)sender).Attributes["ID_UF"];

            if (string.IsNullOrWhiteSpace(ufId))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao obter a UF. Tente novamente.");
            }
            else
            {
                Response.Redirect("GerenciarHierarquia.aspx?Id=" + ufId);
            }
        }
    }
}