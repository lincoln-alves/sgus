using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarFormaAquisicao : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvFormaAquisicao.Rows.Count > 0)
            {
                this.dgvFormaAquisicao.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterFormaAquisicao manterFormaAquisicao = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.FormaAquisicao; }
        }

        protected void dgvFormaAquisicao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterFormaAquisicao = new ManterFormaAquisicao();
                    int idFormaAquisicao = int.Parse(e.CommandArgument.ToString());
                    manterFormaAquisicao.ExcluirFormaAquisicao(idFormaAquisicao);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarFormaAquisicao.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTrilhaFormaAquisicao = int.Parse(e.CommandArgument.ToString());
                //Session.Add("FormaAquisicaoEdit", idTrilhaFormaAquisicao);
                Response.Redirect("EdicaoFormaAquisicao.aspx?Id=" + idTrilhaFormaAquisicao.ToString(), false);
            }
            else if (e.CommandName.Equals("visualizar"))
            {
                int idTrilhaFormaAquisicao = int.Parse(e.CommandArgument.ToString());
                ExibirVisualizar(idTrilhaFormaAquisicao);
            }
            else if (e.CommandName.Equals("copiar"))
            {
                int idTrilhaFormaAquisicao = int.Parse(e.CommandArgument.ToString());
                //Session.Add("FormaAquisicaoEdit", idTrilhaFormaAquisicao);
                Response.Redirect("EdicaoFormaAquisicao.aspx?Id=" + idTrilhaFormaAquisicao.ToString() + "&C=S", false);
            }
        }

        private void ExibirVisualizar(int idFormaAquisicao)
        {
            if (Master != null)
            {
                if (Master.Master != null)
                {
                    var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                    if (pnlbackdrop != null)
                    {
                        var formaAquisicao = (new ManterFormaAquisicao()).ObterFormaAquisicaoPorID(idFormaAquisicao);

                        txtNomeVisualizar.Text = formaAquisicao.Nome;
                        txtDescricaoVisualizar.Text = formaAquisicao.Descricao;
                        txtTipoFormaAquisicao.Text = formaAquisicao.TipoFormaDeAquisicao.ToString();
                        pnlbackdrop.Visible = true;
                        pnlModal.Visible = true;
                    }
                }
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("FormaAquisicaoEdit");
            Response.Redirect("EdicaoFormaAquisicao.aspx");
        }

        private classes.FormaAquisicao ObterObjetoFormaAquisicao()
        {
            classes.FormaAquisicao formaAquisicao = new classes.FormaAquisicao();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                formaAquisicao.Nome = this.txtNome.Text.Trim();

            return formaAquisicao;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                classes.FormaAquisicao formaAquisicao = ObterObjetoFormaAquisicao();
                manterFormaAquisicao = new ManterFormaAquisicao();
                IList<classes.FormaAquisicao> ListaTrilhaFormaAquisicao = manterFormaAquisicao.ObterFormaAquisicoPorFiltro(formaAquisicao);

                if (ListaTrilhaFormaAquisicao != null && ListaTrilhaFormaAquisicao.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaTrilhaFormaAquisicao, this.dgvFormaAquisicao);
                    pnlFormaAquisicao.Visible = true;
                }
                else
                {
                    pnlFormaAquisicao.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void dgvFormaAquisicao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Image imgImagem = (Image)e.Row.FindControl("imgImagem");

                if (imgImagem != null)
                {
                    classes.FormaAquisicao formaAquisicao = (classes.FormaAquisicao)e.Row.DataItem;

                    var usuario = new ManterUsuario().ObterUsuarioLogado();

                    if (usuario.IsGestor() && formaAquisicao.Responsavel != null && formaAquisicao.Responsavel.ID != usuario.ID
                        && formaAquisicao.Uf != null && formaAquisicao.Uf.ID != usuario.UF.ID)
                    {
                        e.Row.Visible = false;
                        return;
                    }

                    if (formaAquisicao != null && !string.IsNullOrWhiteSpace(formaAquisicao.Imagem))
                    {
                        imgImagem.ImageUrl = formaAquisicao.Imagem;
                    }
                    else
                    {
                        imgImagem.Visible = false;
                    }
                }

            }
        }

        protected void lkbBotoesAcesso_PreRender(object sender, EventArgs e)
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado != null)
            {
                LinkButton bntPress = (LinkButton)sender;

                if (usuarioLogado.IsGestor())
                    if (usuarioLogado.UF.ID.ToString() != bntPress.Attributes["id_uf_gestor"] && usuarioLogado.ID.ToString() != bntPress.Attributes["id_responsavel"])
                        bntPress.Attributes.Add("hidden", "hidden");
            }
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            if (Master != null)
            {
                if (Master.Master != null)
                {
                    var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                    if (pnlbackdrop != null)
                    {
                        pnlModal.Visible = false;
                        pnlbackdrop.Visible = false;
                    }
                }
            }
        }
    }
}