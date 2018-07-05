using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.NotificacaoEnvio
{
    public partial class ListarNotificacaoEnvio : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvNotificacaoEnvio.Rows.Count > 0)
            {
                this.dgvNotificacaoEnvio.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        //private Sebrae.Academico.BP.ManterNotificaoEnvio manterNotificacaoEnvio = null;

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
            get { return enumFuncionalidade.NotificacaoEnvio; }
        }

        protected void dgvNotificacaoEnvio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    ManterNotificacaoEnvio manterNotificacaoEnvio = new ManterNotificacaoEnvio();
                    int idNotificacaoEnvio = int.Parse(e.CommandArgument.ToString());
                    manterNotificacaoEnvio.ExcluirNotificacaoEnvio(idNotificacaoEnvio);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarNotificacaoEnvio.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idNotificacaoEnvio = int.Parse(e.CommandArgument.ToString());
                //Session.Add("TurmaEdit", idTipoTurma);
                Response.Redirect("EdicaoNotificacaoEnvio.aspx?Id=" + idNotificacaoEnvio.ToString(), false);
            }
            else if (e.CommandName.Equals("relatorio"))
            {
                Response.Redirect("/Relatorios/Notificacoes/Detalhes.aspx?Id=" + e.CommandArgument.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session["TurmaEdit"] = null;
            Response.Redirect("EdicaoNotificacaoEnvio.aspx");
        }

        private classes.NotificacaoEnvio ObterNotificacaoEnvio()
        {
            classes.NotificacaoEnvio Notificacao = new classes.NotificacaoEnvio();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                Notificacao.Texto = this.txtNome.Text.Trim();

            if (!string.IsNullOrWhiteSpace(this.txtLink.Text))
                Notificacao.Link = this.txtLink.Text.Trim();

            return Notificacao;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    classes.NotificacaoEnvio Notificacao = ObterNotificacaoEnvio();
                    ManterNotificacaoEnvio manterNotificacaoEnvio = new ManterNotificacaoEnvio();

                    using (var manterUsuario = new ManterUsuario())
                    {
                        var usuario = manterUsuario.ObterUsuarioLogado();
                        if (usuario.IsGestor())
                        {
                            Notificacao.Uf = usuario.UF;
                        }
                    }

                    IList<classes.NotificacaoEnvio> ListaNotificacao = manterNotificacaoEnvio.ObterNotificacaoEnvioPorFiltro(Notificacao);
                    WebFormHelper.PreencherGrid(ListaNotificacao, this.dgvNotificacaoEnvio);

                    if (ListaNotificacao != null && ListaNotificacao.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaNotificacao, this.dgvNotificacaoEnvio);
                        pnlNotificacaoEnvio.Visible = true;
                    }
                    else
                    {
                        pnlNotificacaoEnvio.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void dgvNotificacaoEnvio_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (!new ManterUsuario().ObterUsuarioLogado().ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Administrador))
            {
                ((DataControlField)dgvNotificacaoEnvio.Columns
            .Cast<DataControlField>()
            .Where(x => x.HeaderText == "Uf")
            .SingleOrDefault()).Visible = false;
            }
        }
    }
}