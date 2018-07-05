using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.TopicoTematico
{
    public partial class ListarTopicoTematico : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvTopicoTematico.Rows.Count > 0)
            {
                this.dgvTopicoTematico.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        private ManterTrilhaTopicoTematico manterTrilhaTopicoTematico = null;

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
            get { return enumFuncionalidade.TopicoTematico; }
        }

        protected void dgvTopicoTematico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterTrilhaTopicoTematico = new ManterTrilhaTopicoTematico();
                    int idTrilhaTopicoTematico = int.Parse(e.CommandArgument.ToString());
                    manterTrilhaTopicoTematico.ExcluirTrilhaTopicoTematico(idTrilhaTopicoTematico);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarTopicoTematico.aspx");
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
                //Session.Add("TrilhaTopicoTematicoEdit", idTrilhaTopicoTematico);
                Response.Redirect("EdicaoTopicoTematico.aspx?Id=" + idTrilhaTopicoTematico.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("TrilhaTopicoTematicoEdit");
            Response.Redirect("EdicaoTopicoTematico.aspx");
        }

        private TrilhaTopicoTematico ObterObjetoTrilhaTopicoTematico()
        {
            TrilhaTopicoTematico trilhaTopicoTematico = new TrilhaTopicoTematico();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                trilhaTopicoTematico.Nome = this.txtNome.Text.Trim();

            return trilhaTopicoTematico;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    TrilhaTopicoTematico trilhaTopicoTematico = ObterObjetoTrilhaTopicoTematico();
                    manterTrilhaTopicoTematico = new ManterTrilhaTopicoTematico();
                    IList<TrilhaTopicoTematico> ListaTrilhaTopicoTematico = manterTrilhaTopicoTematico.ObterTrilhaTopicoTematicoPorFiltro(trilhaTopicoTematico);

                    if (ListaTrilhaTopicoTematico != null && ListaTrilhaTopicoTematico.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaTrilhaTopicoTematico, this.dgvTopicoTematico);
                        pnlTopicoTematico.Visible = true;
                    }
                    else
                    {
                        pnlTopicoTematico.Visible = false;
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