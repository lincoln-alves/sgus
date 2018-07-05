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

namespace Sebrae.Academico.WebForms.Cadastros.Demanda
{
    public partial class ListarDemandas : System.Web.UI.Page
    {

        ManterProcesso manterProcesso = new ManterProcesso();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                dgvProcesso.Columns[2].Visible = usuarioLogado.IsAdministrador();

                var ufs = new ManterUf().ObterTodosUf();      

                WebFormHelper.PreencherLista(ufs, cbxUF, true);

            }
        }

        private classes.Processo ObterObjetoProcesso()
        {
            var model = new classes.Processo();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                model.Nome = this.txtNome.Text.Trim();

            model.Uf = null;
            if (cbxUF.SelectedValue != "0") {
                model.Uf = new classes.Uf { ID = int.Parse(cbxUF.SelectedValue) };
            }

            if (cbxStatus.SelectedValue != "todos")
            {
                model.Status = Convert.ToBoolean(int.Parse(cbxStatus.SelectedValue)); 
            }

            return model;
        }

       
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    PesquisarRegistros();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void PesquisarRegistros()
        {
            var model = ObterObjetoProcesso();

            var listaProcesso = manterProcesso.ObterPorFiltro(model);

            int tipo;

            if(int.TryParse(cbxTipo.SelectedValue, out tipo))
            {
                listaProcesso = listaProcesso.Where(x => (int)x.Tipo == tipo).ToList();
            }

            WebFormHelper.PreencherGrid(listaProcesso, dgvProcesso);

            if (listaProcesso != null && listaProcesso.Count > 0)
            {
                WebFormHelper.PreencherGrid(listaProcesso, this.dgvProcesso);
                pnlProcesso.Visible = true;
            }
            else
            {
                pnlProcesso.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarDemanda.aspx");
        }

        protected void dgvProcesso_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dgvProcesso_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    manterProcesso = new ManterProcesso();
                    int idProcesso = int.Parse(e.CommandArgument.ToString());

                    try
                    {
                        manterProcesso.ExcluirProcesso(idProcesso);
                    }
                    catch (Exception)
                    {
                        throw new AcademicoException("Não é possível excluir pois há outros dados dependentes deste registro");
                    }

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarDemanda.aspx");
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
                Response.Redirect("EditarDemanda.aspx?Id=" + idProcesso.ToString(), false);
            }
            else if (e.CommandName.Equals("duplicar"))
            {
                int idProcesso = int.Parse(e.CommandArgument.ToString());
                try
                {
                    manterProcesso.DuplicarObjeto(idProcesso);
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro duplicado com sucesso!");
                PesquisarRegistros();
            }

        }
    }
}