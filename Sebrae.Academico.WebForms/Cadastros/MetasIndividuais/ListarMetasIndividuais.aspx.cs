using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.MetasIndividuais
{
    public partial class ListarMetasIndividuais : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvMetaIndividual.Rows.Count > 0)
            {
                this.dgvMetaIndividual.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Session.Remove("IdMetaIndividual");
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
            get { return enumFuncionalidade.MetasIndividuais; }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            IList<MetaIndividual> lstMetaIndividual = (new ManterMetaIndividual()).ObterPorDataValidade(txtNome.Text
                        , LupaUsuario.SelectedUser
                        , (string.IsNullOrWhiteSpace(txtDataValidadeInicial.Text)) ? new DateTime(1, 1, 1) : DateTime.Parse(txtDataValidadeInicial.Text)
                        , (string.IsNullOrWhiteSpace(txtDataValidadeFinal.Text)) ? new DateTime(1, 1, 1) : DateTime.Parse(txtDataValidadeFinal.Text));

            Session["MetasIndividuais"] = lstMetaIndividual.ToList();

            if (lstMetaIndividual.Count > 0) {
                dgvMetaIndividual.DataSource = lstMetaIndividual;
                dgvMetaIndividual.DataBind();
                pnlMetaIndividual.Visible = true;
            } else {
                pnlMetaIndividual.Visible = false;

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Informacao, "Nenhum Registro Encontrado.");
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session.Add("IdMetaIndividual", 0);
            Response.Redirect("EdicaoMetasIndividuais.aspx", false);
        }

        protected void dgvMetaIndividual_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                int idMetaIndividual = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoMetasIndividuais.aspx?Id=" + idMetaIndividual.ToString(), false);
            }
            else if (e.CommandName == "excluir")
            {
                try
                {
                    int idMetaIndividual = int.Parse(e.CommandArgument.ToString());
                    (new ManterMetaIndividual()).ExcluirMetaIndividual(idMetaIndividual);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarMetasIndividuais.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
        }

        protected void dgvMetaIndividual_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var lista = (IList<MetaIndividual>)Session["MetasIndividuais"];
            //lista = lista.Skip(50 * (int)e.NewPageIndex).Take(50).ToList();
            WebFormHelper.PaginarGrid(lista, dgvMetaIndividual, e.NewPageIndex);
        }
    }
}