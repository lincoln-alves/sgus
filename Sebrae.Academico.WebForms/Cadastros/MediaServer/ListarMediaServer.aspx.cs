using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Web;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.MediaServer
{
    public partial class ListarMediaServer : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvFileServer.Rows.Count > 0)
            {
                this.dgvFileServer.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterFileServer manterFileServer = null;

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
            get { return enumFuncionalidade.MediaServer; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        private classes.FileServer ObterObjetoFileServer()
        {
            var mediaServer = new classes.FileServer();
            if (!string.IsNullOrWhiteSpace(this.txtNome.Text)) mediaServer.NomeDoArquivoOriginal = this.txtNome.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtID.Text))
            {
                var idBusca = 0;
                if (!int.TryParse(this.txtID.Text, out idBusca))
                {
                    throw new AcademicoException("Código inválido. Informe apenas números.");
                }
                mediaServer.ID = idBusca;
            }
            mediaServer.MediaServer = true;

            return mediaServer;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarMediaServer.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var mediaServer = ObterObjetoFileServer();
                manterFileServer = new ManterFileServer();
                var listaFileServer = manterFileServer.ObterFileServerPorFiltro(mediaServer, true);

                if (listaFileServer != null && listaFileServer.Count > 0)
                {
                    WebFormHelper.PreencherGrid(listaFileServer, this.dgvFileServer);
                    pnlFileServer.Visible = true;
                }
                else
                {
                    pnlFileServer.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void dgvFileServer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                HyperLink hplnkCaminhoReduzido = (HyperLink)e.Row.FindControl("hplnkCaminhoReduzido");

                FileServer fileServer = (FileServer)e.Row.DataItem;

                if (hplnkCaminhoReduzido != null)
                {
                    string caminhoArquivoAshx = string.Format(@"/MediaServer.ashx?Identificador={1}", HttpContext.Current.Request.Url, fileServer.ID);
                    string urlCompleta = HttpContext.Current.Request.Url.ToString();
                    urlCompleta = urlCompleta.Replace(HttpContext.Current.Request.RawUrl, string.Concat("/", caminhoArquivoAshx));

                    string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                    string urlReduzida = string.Format("{0}/ms/{1}", baseUrl, fileServer.ID);

                    hplnkCaminhoReduzido.Text = urlReduzida;
                    hplnkCaminhoReduzido.NavigateUrl = urlReduzida;
                }

                //NomeDoArquivoOriginal
                Label lblNome = (Label)e.Row.FindControl("lblNome");

                if (lblNome != null)
                {
                    lblNome.Text = fileServer.NomeDoArquivoOriginal;
                }
            }

        }

        protected void dgvFileServer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterFileServer = new ManterFileServer();
                    int idFileServer = int.Parse(e.CommandArgument.ToString());
                    manterFileServer.ExcluirFileServer(idFileServer);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarMediaServer.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idFileServer = int.Parse(e.CommandArgument.ToString());
                //Session.Add("FileServerEdit", idFileServer);
                Response.Redirect("EditarMediaServer.aspx?Id=" + idFileServer.ToString(), false);
            }
        }

        protected void lkbBotoesAcesso_PreRender(object sender, EventArgs e)
        {
            var usuarioLogado = new BMUsuario().ObterUsuarioLogado();

            if (usuarioLogado != null)
            {
                LinkButton bntPress = (LinkButton)sender;

                if (!usuarioLogado.IsAdministrador() && !usuarioLogado.IsGestor())
                {
                    bntPress.Attributes.Add("hidden", "hidden");
                }

            }
        }
    }
}