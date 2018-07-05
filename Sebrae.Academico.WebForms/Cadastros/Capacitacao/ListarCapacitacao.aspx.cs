using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Capacitacao
{
    public partial class ListarCapacitacao : System.Web.UI.Page
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvCapacitacao.Rows.Count > 0)
            {
                this.dgvCapacitacao.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterCapacitacao manterCapacitacao = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var bmCapacitacao = new BMCapacitacao();
                WebFormHelper.PreencherLista(new BMPrograma().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlPrograma, true);
            }   
        }

        protected void dgvCapacitacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterCapacitacao = new ManterCapacitacao();
                    int idCapacitacao = int.Parse(e.CommandArgument.ToString());
                    manterCapacitacao.ExcluirCapacitacao(idCapacitacao);   
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "/Cadastros/Capacitacao/ListarCapacitacao.aspx");
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idCapacitacao = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoCapacitacao.aspx?Id=" + idCapacitacao.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //@todo Mudar o endereco
            Response.Redirect("EdicaoCapacitacao.aspx");
        }

        //private classes.Capacitacao ObterObjetoCapacitacao()
        //{
        //    classes.Programa programa = new classes.Programa();

        //    if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
        //        programa.Nome = this.txtNome.Text.Trim();

        //    return programa;
        //}

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    classes.Capacitacao capacitacao = ObterObjetoCapacitacao();
                    manterCapacitacao = new ManterCapacitacao();
                    //IList<classes.Capacitacao> ListaCapacitacao = manterCapacitacao.ObterCapacitacaoPorFiltro(Capacitacao);
                    IList<classes.Capacitacao> ListaCapacitacao = manterCapacitacao.ObterPorFiltro(capacitacao);
                    //WebFormHelper.PreencherGrid(ListaCapacitacao, this.dgvCapacitacao);

                    if (ListaCapacitacao != null && ListaCapacitacao.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaCapacitacao, this.dgvCapacitacao);
                        pnlCapacitacao.Visible = true;
                    }
                    else
                    {
                        pnlCapacitacao.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private classes.Capacitacao ObterObjetoCapacitacao()
        {
            classes.Capacitacao capacitacao = new classes.Capacitacao();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                capacitacao.Nome = this.txtNome.Text.Trim();

            if (ddlPrograma.SelectedIndex > 0)
                capacitacao.Programa.ID = int.Parse(ddlPrograma.SelectedValue);

            return capacitacao;
        }
    }
}