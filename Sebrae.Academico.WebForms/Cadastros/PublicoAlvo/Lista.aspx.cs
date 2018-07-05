using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.PublicoAlvo
{
    public partial class Lista : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
		
        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.PublicoAlvo; }
        }
		
        protected void dgvPublicoAlvo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var bm = new BMPublicoAlvo();
                    int idPublicoAlvo = int.Parse(e.CommandArgument.ToString());
                    bm.Excluir(idPublicoAlvo);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "Lista.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idPublicoAlvo = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("Editar.aspx?Id=" + idPublicoAlvo.ToString(), false);
            }
        }
		
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Editar.aspx");
        }
		
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                classes.PublicoAlvo pPublicoAlvo = new classes.PublicoAlvo { Nome = txtNome.Text };
                var manterPublicoAlvo = new BMPublicoAlvo();
                IList<classes.PublicoAlvo> ListaTrilhaPublicoAlvo = manterPublicoAlvo.ObterPorFiltro(pPublicoAlvo);

                if (ListaTrilhaPublicoAlvo != null && ListaTrilhaPublicoAlvo.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaTrilhaPublicoAlvo, this.dgvPublicoAlvo);
                    pnlPublicoAlvo.Visible = true;
                }
                else
                {
                    pnlPublicoAlvo.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
		
        protected void dgvPublicoAlvo_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                var publicoAlvo = (classes.PublicoAlvo)e.Row.DataItem;
				
                var litOfertasVinculadas = (Literal)e.Row.FindControl("litOfertasVinculadas");
				
                litOfertasVinculadas.Text = publicoAlvo.ListaOferta.Count.ToString();
				
				var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
				
				if(usuarioLogado.IsGestor() && (publicoAlvo.UF == null || usuarioLogado.UF.ID != publicoAlvo.UF.ID))
				{
					var lkbEditar = (LinkButton)e.Row.FindControl("lkbEditar");
					var lkbExcluir = (LinkButton)e.Row.FindControl("lkbExcluir");

				    lkbEditar.Visible = false;
					lkbExcluir.Visible = false;
				}
            }
        }
    }
}