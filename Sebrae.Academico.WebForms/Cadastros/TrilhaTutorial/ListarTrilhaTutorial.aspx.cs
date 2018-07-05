using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System.Linq;
using System.Web.Script.Serialization;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarTrilhaTutorial : Page
    {        
        private ManterTrilhaTutorial manterTrilhaTutorial = null;                

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                this.PreencherComponenteCategoriaTutorialTrilha();
            }            
        }

        protected void Excluir_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int id = int.Parse(btn.CommandArgument);
                manterTrilhaTutorial = new ManterTrilhaTutorial();
                manterTrilhaTutorial.ExcluirTrilhaTutorial(id);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarTrilhaTutorial.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }


        protected void Editar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;
            Response.Redirect("/Cadastros/TrilhaTutorial/EdicaoTrilhaTutorial.aspx?Id=" + id, false);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("TrilhaTutorialEdit");
            Response.Redirect("EdicaoTrilhaTutorial.aspx");
        }

        private classes.TrilhaTutorial ObterObjetoTrilhaTutorial()
        {
            classes.TrilhaTutorial TrilhaTutorial = new classes.TrilhaTutorial();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                TrilhaTutorial.Nome = this.txtNome.Text.Trim();

            if (this.ddlCategoria.SelectedValue!="")
                TrilhaTutorial.Categoria = (enumCategoriaTrilhaTutorial)int.Parse(ddlCategoria.SelectedValue);

            return TrilhaTutorial;
        }

        protected void mostrarResultadosDaPesquisa()
        {
            classes.TrilhaTutorial TrilhaTutorial = ObterObjetoTrilhaTutorial();
            manterTrilhaTutorial = new ManterTrilhaTutorial();
            List<classes.TrilhaTutorial> ListaTrilhaTutorial = manterTrilhaTutorial.ObterTrilhaTutorialPorFiltro(TrilhaTutorial).ToList();

            if (ListaTrilhaTutorial != null && ListaTrilhaTutorial.Count > 0)
            {
                rptTrilhaTutorial.DataSource = ListaTrilhaTutorial;
                rptTrilhaTutorial.DataBind();
                pnlEtapas.Visible = true;
            }
            else
            {
                pnlEtapas.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarResultadosDaPesquisa();                
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        protected void Repeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic item = e.Item.DataItem as dynamic;

                ((LinkButton)e.Item.FindControl("Editar")).CommandArgument = (e.Item.DataItem as classes.TrilhaTutorial).ID.ToString();
                ((LinkButton)e.Item.FindControl("Excluir")).CommandArgument = (e.Item.DataItem as classes.TrilhaTutorial).ID.ToString();                
            }
        }

        private void PreencherComponenteCategoriaTutorialTrilha()
        {

            var start = new ListItem()
            {
                Text = "Selecione uma categoria",
                Value = ""
            };

            ddlCategoria.Items.Add(start);

            var campos = Enum.GetValues(typeof(enumCategoriaTrilhaTutorial)).Cast<enumCategoriaTrilhaTutorial>().Select(campo =>
            new ListItem()
            {
                Text = campo.GetDescription(),
                Value = ((int)campo).ToString()
            }).OrderBy(campo => campo.Text).ToArray();

            ddlCategoria.Items.AddRange(campos);
        }

        protected void btnSalvaOrdem_Click(object sender, EventArgs e)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                dynamic obj = serializer.Deserialize(hdnOrdenacao.Value, typeof(object));
                if (obj != null)
                {
                    manterTrilhaTutorial = new ManterTrilhaTutorial();
                    manterTrilhaTutorial.AlterarOrdemTrilhaTutorial(obj);

                    // Verificar se existe uma forma de não causar o postBack desse clique e assim remover a linha abaixo
                    mostrarResultadosDaPesquisa();
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

    }
}