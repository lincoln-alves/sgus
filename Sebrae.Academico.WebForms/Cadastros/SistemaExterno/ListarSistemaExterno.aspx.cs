using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarSistemaExterno : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvSistemaExterno.Rows.Count > 0)
            {
                this.dgvSistemaExterno.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterSistemaExterno manterSistemaExterno = null;

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

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.SistemaExterno; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void dgvSistemaExterno_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterSistemaExterno = new ManterSistemaExterno();
                    int idSistemaExterno = int.Parse(e.CommandArgument.ToString());
                    manterSistemaExterno.ExcluirSistemaExterno(idSistemaExterno);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarSistemaExterno.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTrilhaTopicoTematico = int.Parse(e.CommandArgument.ToString());
                //Session.Add("SistemaExternoEdit", idTrilhaTopicoTematico);
                Response.Redirect("EdicaoSistemaExterno.aspx?Id=" + idTrilhaTopicoTematico.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoSistemaExterno.aspx");
        }

        private SistemaExterno ObterObjetoSistemaExterno()
        {
            SistemaExterno sistemaExterno = new SistemaExterno();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                sistemaExterno.Nome = this.txtNome.Text.Trim();

            return sistemaExterno;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                SistemaExterno sistemaExterno = ObterObjetoSistemaExterno();
                manterSistemaExterno = new ManterSistemaExterno();
                IList<SistemaExterno> ListaSistemaExterno = manterSistemaExterno.ObterSistemaExternoPorFiltro(sistemaExterno);

                if (ListaSistemaExterno != null && ListaSistemaExterno.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaSistemaExterno, this.dgvSistemaExterno);
                    pnlSistemaExterno.Visible = true;
                }
                else
                {
                    pnlSistemaExterno.Visible = false;
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