using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarUsuário : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvUsuario.Rows.Count > 0)
            {
                this.dgvUsuario.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    WebFormHelper.PreencherLista(new ManterUf().ObterDoUsuarioLogado(), cbxFiltroUF, false, true);
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
            get { return enumFuncionalidade.Programa; }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {

            try 
            {
                dgvUsuario.DataSource = new ManterUsuario().ObterPorFiltro(new Usuario() { Nome = txtNome.Text, CPF = txtCPF.Text.Replace(".",string.Empty).Replace("-",string.Empty).Replace("_",string.Empty),
                                                                                          UF = new ManterUf().ObterUfPorID(int.Parse(String.IsNullOrWhiteSpace(cbxFiltroUF.SelectedValue) ? "0" : cbxFiltroUF.SelectedValue))});
                dgvUsuario.DataBind();
            }
            catch (Exception ex) 
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

            
        }

        protected void dgvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idUsuario = int.Parse(e.CommandArgument.ToString());
            //Session.Add("UsuarioEdit", idUsuario);
            Response.Redirect("EdicaoUsuario.aspx?Id=" + idUsuario.ToString(), false);
        }

        protected void btnCadastrarUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("InclusaoUsuario.aspx", false);
        }

        
    }
}