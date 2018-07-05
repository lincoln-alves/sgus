using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Web.UI;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.Cadastros.TrilhaNivel
{
    public partial class ListarTrilhaNivel : System.Web.UI.Page
    {
        public IList<classes.TrilhaNivel> NiveisBusca
        {
            get
            {
                return Session["NiveisTrilha"] != null ? Session["NiveisTrilha"] as List<classes.TrilhaNivel> :
                    new List<classes.TrilhaNivel>();
            }

            set
            {
                Session["NiveisTrilha"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack) return;

                ViewState["_Trilhas"] = Helpers.Util.ObterListaAutocomplete(new ManterTrilha().ObterTodasTrilhasIQueryable());
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void txtTrilha_TextChanged(object sender, EventArgs e)
        {
            int idTrilha;
            if (int.TryParse(txtTrilha.Text, out idTrilha))
            {
                var niveis = new ManterTrilhaNivel().ObterPorTrilha(idTrilha);
                PreencherGridNiveis(niveis);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            classes.TrilhaNivel nivel = new classes.TrilhaNivel();

            int idTrilha;
            if (int.TryParse(txtTrilha.Text, out idTrilha))
            {
                nivel.Trilha = new ManterTrilha().ObterTrilhaPorId(idTrilha);
            }

            nivel.Nome = txtNome.Text;

            var niveis = new ManterTrilhaNivel().ObterTrilhaNivelPorFiltro(nivel);

            PreencherGridNiveis(niveis);
            NiveisBusca = niveis;
        }

        private void PreencherGridNiveis(IList<classes.TrilhaNivel> niveis)
        {
            if (niveis.Count > 0)
            {
                pnlResultadoBusca.Visible = true;
                WebFormHelper.PreencherGrid(niveis, dgvTrilhaNivel);
            }
            else
            {
                pnlResultadoBusca.Visible = false;

                dgvTrilhaNivel.DataSource = null;
                dgvTrilhaNivel.DataBind();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoTrilhaNivel.aspx");
        }

        protected void dgvTrilhaNivel_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idTrilhaNivel;

            switch (e.CommandName)
            {
                case "editar":
                    idTrilhaNivel = int.Parse(e.CommandArgument.ToString());
                    //Session.Add("ItemTrilhaEdit", idTrilhaTopicoTematico);
                    Response.Redirect("EdicaoTrilhaNivel.aspx?Id=" + idTrilhaNivel.ToString(), false);
                    break;
                case "excluir":
                    var manterTrilhaNivel = new ManterTrilhaNivel();
                    idTrilhaNivel = int.Parse(e.CommandArgument.ToString());
                    manterTrilhaNivel.ExcluirTrilhaNivel(idTrilhaNivel);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarTrilhaNivel.aspx");
                    break;
                default:
                    break;
            }
        }

        protected void dgvTrilhaNivel_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid(NiveisBusca, dgvTrilhaNivel, e.NewPageIndex);
        }
    }
}