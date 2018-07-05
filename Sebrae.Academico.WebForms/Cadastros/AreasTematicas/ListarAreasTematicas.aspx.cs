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

namespace Sebrae.Academico.WebForms.Cadastros.AreasTematicas
{
    public partial class ListarAreasTematicas : PageBase
    {
        private readonly ManterAreaTematica _manterAreaTematica = new ManterAreaTematica();
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvAreasTematicas.Rows.Count > 0) this.dgvAreasTematicas.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.LogarAcessoFuncionalidade();
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.AreasTematicas; }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarAreaTematica.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                AreaTematica filtro = null;
                if (!string.IsNullOrEmpty(txtNome.Text)) { filtro = new AreaTematica { Nome = txtNome.Text }; }
                var listaAreasTematicas = _manterAreaTematica.ObterPorFiltro(filtro);

                if (listaAreasTematicas != null && listaAreasTematicas.Count > 0)
                {
                    WebFormHelper.PreencherGrid(listaAreasTematicas, this.dgvAreasTematicas);
                    pnlAreatematica.Visible = true;
                }
                else
                {
                    pnlAreatematica.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void dgvAreasTematicas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var id = int.Parse(e.CommandArgument.ToString());
                    _manterAreaTematica.ExcluirAreaTematica(id);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "/Cadastros/AreasTematicas/ListarAreasTematicas.aspx");
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possivel excluir o registro!", "/Cadastros/AreasTematicas/ListarAreasTematicas.aspx");
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                Response.Redirect("EditarAreaTematica.aspx?Id=" + e.CommandArgument, false);
            }
        }

        protected void dgvAreasTematicas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

            }
        }
    }
}