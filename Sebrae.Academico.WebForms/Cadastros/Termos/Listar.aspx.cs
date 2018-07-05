using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Termos
{
    public partial class Listar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void dgvTermoAceite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var idTermoAceite = int.Parse(e.CommandArgument.ToString());

            var manterAceite = new ManterTermoAceite();

            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterAceite.Excluir(idTermoAceite);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "Listar.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                Response.Redirect("Listar.aspx");
            }
            else if (e.CommandName.Equals("editar"))
            {
                Response.Redirect("Editar.aspx?Id=" + idTermoAceite, false);
            }
            else if (e.CommandName.Equals("duplicar"))
            {
                Response.Redirect("Editar.aspx?Id=" + idTermoAceite + "&Duplicar=1", false);
            }
            else if (e.CommandName.Equals("visualizar"))
            {
                if (Master != null)
                {
                    if (Master.Master != null)
                    {
                        var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                        if (pnlbackdrop != null)
                        {
                            pnlbackdrop.Visible =
                            pnlVisualizar.Visible = true;

                            PreencherVisualizacaoTermoAceite(manterAceite.ObterTermoAceitePorID(idTermoAceite));

                            return;
                        }
                    }
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Não foi possível obter o Termo para exibição. Tente novamente.");
            }
        }

        private void PreencherVisualizacaoTermoAceite(classes.TermoAceite termoAceite)
        {
            if (termoAceite != null)
            {
                ltrNome.Text = termoAceite.Nome;
                ltrTexto.Text = termoAceite.Texto;
                ltrPolitica.Text = termoAceite.PoliticaConsequencia;

                if (termoAceite.Usuario != null)
                {
                    divUF.Visible = true;
                    ltrUf.Text = termoAceite.Usuario.UF.Nome + " - " + termoAceite.Usuario.UF.Sigla;
                }
                else
                {
                    divUF.Visible = false;
                }

                rptCategorias.DataSource = termoAceite.ListaCategoriaConteudo;
                rptCategorias.DataBind();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Editar.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        { 
            if (!Page.IsValid) return;

            try
            {
                var manterTermoAceite = new ManterTermoAceite();
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                var listaTermoAceite = (string.IsNullOrWhiteSpace(txtNome.Text))
                    ? manterTermoAceite.ObterTodosTermoAceite().AsQueryable()
                    : manterTermoAceite.ObterPorNome(txtNome.Text.Trim()).AsQueryable();

                if (listaTermoAceite.Any())
                {
                    WebFormHelper.PreencherGrid(listaTermoAceite.ToList(), dgvTermoAceite);
                    pnlPrograma.Visible = true;
                }
                else
                {
                    pnlPrograma.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public classes.Usuario UsuarioLogado { get; set; }

        protected void dgvTermoAceite_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                var termoAceite = (classes.TermoAceite)e.Row.DataItem;

                // Criar comportamento de singleton com o usuário logado para não ficar consultando sempre.
                UsuarioLogado = UsuarioLogado ?? new ManterUsuario().ObterUsuarioLogado();

                // Esconder os botões de editar e excluir caso o gestor não seja o criador do termo.
                if (UsuarioLogado.IsGestor() && (termoAceite.Usuario == null || UsuarioLogado.UF.ID != termoAceite.Uf.ID))
                {
                    var lkbEditar = (LinkButton)e.Row.FindControl("lkbEditar");
                    var lkbExcluir = (LinkButton)e.Row.FindControl("lkbExcluir");

                    lkbEditar.Visible =
                    lkbExcluir.Visible = false;
                }
            }
        }

        protected void Fechar_OnServerClick(object sender, EventArgs e)
        {
            if (Master != null)
            {
                if (Master.Master != null)
                {
                    var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                    if (pnlbackdrop != null)
                    {
                        pnlbackdrop.Visible = false;
                    }
                }
            }

            pnlVisualizar.Visible = false;
        }

        protected void rptCategorias_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var categoria = ((classes.TermoAceiteCategoriaConteudo)e.Item.DataItem).CategoriaConteudo;

                var ltrCategoria = (Literal)e.Item.FindControl("ltrCategoria");

                ltrCategoria.Text = categoria.Sigla + " - " + categoria.Nome;
            }
        }
    }
}