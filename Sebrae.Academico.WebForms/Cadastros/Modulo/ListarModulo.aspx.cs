using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Modulo
{
    public partial class ListarModulo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var bmCapacitacao = new BMCapacitacao();
                WebFormHelper.PreencherLista(new BMPrograma().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlPrograma, true);
            }   
        }

        protected void ddlPrograma_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrograma.SelectedIndex > 0)
            {
                ddlCapacitacao.Enabled = true;
                var filtro = new classes.Capacitacao();
                filtro.Programa.ID = int.Parse(ddlPrograma.SelectedValue);
                WebFormHelper.PreencherLista(new BMCapacitacao().ObterPorFiltro(filtro).ToList(), ddlCapacitacao, true, false);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            classes.Modulo modulo = ObterObjetoModulo();
            IList<classes.Modulo> listaModulo = new BMModulo().ObterPorFiltro(modulo);

            if (listaModulo != null && listaModulo.Count > 0)
            {
                WebFormHelper.PreencherGrid(listaModulo, this.gvModulo);
                pnlCapacitacao.Visible = true;
            }
            else
            {
                pnlCapacitacao.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
            }
        }

        private classes.Modulo ObterObjetoModulo()
        {
            var modulo = new classes.Modulo();

            if (!string.IsNullOrEmpty(txtNome.Text))
                modulo.Nome = txtNome.Text;

            if (ddlPrograma.SelectedIndex > 0 && ddlCapacitacao.SelectedIndex == 0)
                modulo.Capacitacao.Programa.ID = int.Parse(ddlPrograma.SelectedValue);

            if (ddlCapacitacao.SelectedIndex > 0)
                modulo.Capacitacao.ID = int.Parse(ddlCapacitacao.SelectedValue);

            return modulo;
        }
        
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarModulo.aspx");
        }

        protected void dgvModulo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idModulo = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("editar"))
            {
                Response.Redirect("EditarModulo.aspx?Id=" + idModulo.ToString(), false);
            }
            if (e.CommandName.Equals("excluir"))
            {
                var bm = new BMModulo();
                bm.Excluir(bm.ObterPorId(idModulo));
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso", "ListarModulo.aspx");
            }
        }
    }
}