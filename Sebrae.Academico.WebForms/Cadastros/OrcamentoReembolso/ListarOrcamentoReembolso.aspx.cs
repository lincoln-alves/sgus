using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;

namespace Sebrae.Academico.WebForms.Cadastros.OrcamentoReembolso
{
    public partial class ListarOrcamentoReembolso : Page
    {
        private readonly ManterOrcamentoReembolso _manterOrcamentoReembolso = new ManterOrcamentoReembolso();

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvOrcamentoReembolso.Rows.Count > 0) this.dgvOrcamentoReembolso.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarOrcamentoReembolso.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var listaOrcamentoReembolso = _manterOrcamentoReembolso.ObterOrcamento();

                if (listaOrcamentoReembolso != null && listaOrcamentoReembolso.Count() > 0)
                {
                    WebFormHelper.PreencherGrid(listaOrcamentoReembolso, dgvOrcamentoReembolso);
                    pnlOrcamentoReembolso.Visible = true;
                }
                else
                {
                    pnlOrcamentoReembolso.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void dgvOrcamentoReembolso_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var id = int.Parse(e.CommandArgument.ToString());
                    _manterOrcamentoReembolso.Remover(id);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx");
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possivel excluir o registro!", "/Cadastros/OrcamentoReembolso/ListarOrcamentoReembolso.aspx");
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                Response.Redirect("EditarOrcamentoReembolso.aspx?Id=" + e.CommandArgument, false);
            }
        }
    }
}