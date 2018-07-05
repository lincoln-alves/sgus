using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Tag
{
    public partial class ListarTag : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvTag.Rows.Count > 0)
            {
                this.dgvTag.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterTag manterTag = null;

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
            get { return enumFuncionalidade.Tag; }
        }

        protected void dgvTag_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    manterTag = new ManterTag();
                    int idTag = int.Parse(e.CommandArgument.ToString());
                    manterTag.ExcluirTag(idTag);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarTag.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao processar a solicitação. Favor verificar registros dependentes");
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTag = int.Parse(e.CommandArgument.ToString());
                //Session.Add("TagEdit", idTag);
                Response.Redirect("EdicaoTag.aspx?Id=" + idTag.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session.Remove("TagEdit");
            Response.Redirect("EdicaoTag.aspx");
        }

        private Classes.Tag ObterObjetoTag()
        {
            Classes.Tag tag = new Classes.Tag();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
            {
                tag.Nome = this.txtNome.Text.Trim();
            }

            return tag;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Classes.Tag tag = ObterObjetoTag();
                    manterTag = new ManterTag();
                    IList<Classes.Tag> ListaTag = manterTag.ObterTagPorFiltro(tag);
                    WebFormHelper.PreencherGrid(ListaTag, this.dgvTag);

                    if (ListaTag != null && ListaTag.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaTag, this.dgvTag);
                        pnlTag.Visible = true;
                    }
                    else
                    {
                        pnlTag.Visible = false;
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