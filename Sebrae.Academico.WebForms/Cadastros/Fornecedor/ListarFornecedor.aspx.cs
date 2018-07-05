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
    public partial class ListarFornecedor : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvFornecedor.Rows.Count > 0)
            {
                this.dgvFornecedor.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterFornecedor manterFornecedor = null;

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
            get { return enumFuncionalidade.Fornecedor; }
        }

        protected void dgvFornecedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterFornecedor = new ManterFornecedor();
                    int idFornecedor = int.Parse(e.CommandArgument.ToString());
                    manterFornecedor.ExcluirFornecedor(idFornecedor);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarFornecedor.aspx");
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
                //Session.Add("FornecedorEdit", idTrilhaFormaAquisicao);
                Response.Redirect("EdicaoFornecedor.aspx?Id=" + idTrilhaFormaAquisicao.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("FornecedorEdit");
            Response.Redirect("EdicaoFornecedor.aspx");
        }

        private Fornecedor ObterObjetoFornecedor()
        {
            Fornecedor fornecedor = new Fornecedor();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                fornecedor.Nome = this.txtNome.Text.Trim();

            return fornecedor;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                Fornecedor formaAquisicao = ObterObjetoFornecedor();
                manterFornecedor = new ManterFornecedor();
                IList<Fornecedor> ListaTrilhaFormaAquisicao = manterFornecedor.ObterFornecedorPorFiltro(formaAquisicao);

                if (ListaTrilhaFormaAquisicao != null && ListaTrilhaFormaAquisicao.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaTrilhaFormaAquisicao, this.dgvFornecedor);
                    pnlFornecedor.Visible = true;
                }
                else
                {
                    pnlFornecedor.Visible = false;
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