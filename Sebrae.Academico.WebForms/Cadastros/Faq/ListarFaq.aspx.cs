using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.Cadastros.Faq
{
    public partial class ListarFaq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    PreencherCampos();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherCampos()
        {
            var manterFaq = new ManterTrilhaFaq();
            dvgFaq.DataSource = manterFaq.ObterTodos().ToList();
            dvgFaq.DataBind();
        }

        protected void dvgFaq_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var manter = new ManterTrilhaFaq();
                    int idFaq = int.Parse(e.CommandArgument.ToString());
                    var faq = manter.ObterPorId(idFaq);
                    manter.Excluir(faq);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarFaq.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idFaq = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoFaq.aspx?Id=" + idFaq.ToString(), false);
            }
            else if (e.CommandName.Equals("visualizar"))
            {
                int idFaq = int.Parse(e.CommandArgument.ToString());
                ExibirVisualizar(idFaq);
            }
        }

        private void ExibirVisualizar(int idFaq)
        {
            if (Master != null)
            {
                if (Master.Master != null)
                {
                    var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                    if (pnlbackdrop != null)
                    {
                        var trilhaFaq = (new ManterTrilhaFaq()).ObterPorId(idFaq);

                        txtNomeVisualizar.Text = trilhaFaq.Nome;
                        txtDescricaoVisualizar.Text = trilhaFaq.Descricao;
                        pnlbackdrop.Visible = true;
                        pnlModal.Visible = true;
                    }
                }
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            IList<TrilhaFaq> faqs;

            if (!string.IsNullOrEmpty(txtNome.Text))
            {
                var model = new TrilhaFaq()
                {
                    Nome = txtNome.Text
                };

                faqs = new ManterTrilhaFaq().ObterPorFiltro(model);
            }
            else
            {
                faqs = new ManterTrilhaFaq().ObterTodos();
            }

            if (faqs != null && faqs.Count > 0)
            {
                dvgFaq.DataSource = faqs;
                dvgFaq.DataBind();
            }

            pnlFaq.Visible = true;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoFaq.aspx");
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