using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarPrograma : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvPrograma.Rows.Count > 0)
            {
                this.dgvPrograma.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterPrograma manterPrograma = null;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Programa; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

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

        protected void dgvPrograma_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try {
                    manterPrograma = new ManterPrograma();
                    var idPrograma = int.Parse(e.CommandArgument.ToString());
                    manterPrograma.ExcluirPrograma(idPrograma);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso", "ListarPrograma.aspx");
                } catch (AcademicoException ex) {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                } catch (Exception ex) {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                Response.Redirect("ListarPrograma.aspx");
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idPrograma = int.Parse(e.CommandArgument.ToString());
                //Session.Add("ProgramaEdit", idPrograma);
                Response.Redirect("EdicaoPrograma.aspx?Id=" + idPrograma.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session["ProgramaEdit"] = null;
            Response.Redirect("EdicaoPrograma.aspx");
        }

        private classes.Programa ObterObjetoPrograma()
        {
            classes.Programa programa = new classes.Programa();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                programa.Nome = this.txtNome.Text.Trim();

            return programa;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    classes.Programa Programa = ObterObjetoPrograma();
                    manterPrograma = new ManterPrograma();
                    IList<classes.Programa> ListaPrograma = manterPrograma.ObterProgramaPorFiltro(Programa);
                    WebFormHelper.PreencherGrid(ListaPrograma, this.dgvPrograma);

                    if (ListaPrograma != null && ListaPrograma.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaPrograma, this.dgvPrograma);
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
        }
    }
}