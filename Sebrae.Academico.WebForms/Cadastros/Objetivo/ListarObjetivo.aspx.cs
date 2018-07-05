using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarObjetivo : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvObjetivo.Rows.Count > 0)
            {
                this.dgvObjetivo.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterObjetivo manterObjetivo = null;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Objetivo; }
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

        protected void dgvObjetivo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterObjetivo = new ManterObjetivo();
                    int idObjetivo = int.Parse(e.CommandArgument.ToString());
                    manterObjetivo.ExcluirObjetivo(idObjetivo);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarObjetivo.aspx");
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
                //Session.Add("ObjetivoEdit", idTrilhaTopicoTematico);
                Response.Redirect("EdicaoObjetivo.aspx?Id=" + idTrilhaTopicoTematico.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("ObjetivoEdit");
            Response.Redirect("EdicaoObjetivo.aspx");
        }

        private classes.Objetivo ObterObjetoObjetivo()
        {
            classes.Objetivo objetivo = new classes.Objetivo();

            if (!string.IsNullOrWhiteSpace(this.txtObjetivo.Text))
                objetivo.Nome = this.txtObjetivo.Text.Trim();

            return objetivo;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                classes.Objetivo Objetivo = ObterObjetoObjetivo();
                manterObjetivo = new ManterObjetivo();
                IList<classes.Objetivo> ListaObjetivo = manterObjetivo.ObterObjetivoPorFiltro(Objetivo);
                base.LogarAcessoFuncionalidade();

                if (ListaObjetivo != null && ListaObjetivo.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaObjetivo, this.dgvObjetivo);
                    pnlObjetivo.Visible = true;
                }
                else
                {
                    pnlObjetivo.Visible = false;
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