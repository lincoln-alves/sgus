using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Etapa
{
    public partial class ListarEtapa : System.Web.UI.Page
    {

        ManterEtapa manterEtapa = new ManterEtapa();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private classes.Etapa ObterObjetoEtapa()
        {
            classes.Etapa model = new classes.Etapa();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                model.Nome = this.txtNome.Text.Trim();

            return model;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    classes.Etapa model = ObterObjetoEtapa();
                    IList<classes.Etapa> ListaEtapa = manterEtapa.ObterPorFiltro(model);
                    WebFormHelper.PreencherGrid(ListaEtapa, this.dgvEtapa);

                    if (ListaEtapa != null && ListaEtapa.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaEtapa, this.dgvEtapa);
                        pnlEtapa.Visible = true;
                    }
                    else
                    {
                        pnlEtapa.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarEtapa.aspx");
        }

        protected void dgvEtapa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dgvEtapa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    manterEtapa = new ManterEtapa();
                    int idModel= int.Parse(e.CommandArgument.ToString());
                    manterEtapa.Excluir(idModel);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarEtapa.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idProcesso = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EditarEtapa.aspx?Id=" + idProcesso.ToString(), false);
            }
        }
    }
}