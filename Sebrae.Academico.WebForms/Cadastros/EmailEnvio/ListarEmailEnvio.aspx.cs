using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.EmailEnvio
{
    public partial class ListarEmailEnvio : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvEmailEnvio.Rows.Count > 0)
            {
                this.dgvEmailEnvio.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        //private Sebrae.Academico.BP.ManterNotificaoEnvio manterEmailEnvio = null;

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
            get { return enumFuncionalidade.EmailEnvio; }
        }

        protected void dgvEmailEnvio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    ManterEmailEnvio manterEmailEnvio = new ManterEmailEnvio();
                    int idEmailEnvio = int.Parse(e.CommandArgument.ToString());
                    manterEmailEnvio.ExcluirEmailEnvio(idEmailEnvio);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarEmailEnvio.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idEmailEnvio = int.Parse(e.CommandArgument.ToString());
                //Session.Add("TurmaEdit", idTipoTurma);
                Response.Redirect("EdicaoEmailEnvio.aspx?Id=" + idEmailEnvio.ToString(), false);
            }
            else if (e.CommandName.Equals("relatorio"))
            {
                Response.Redirect("/Relatorios/Emails/Detalhes.aspx?Id=" + e.CommandArgument.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoEmailEnvio.aspx");
        }

        private classes.EmailEnvio ObterEmailEnvio()
        {
            classes.EmailEnvio Email = new classes.EmailEnvio();

            if (!string.IsNullOrWhiteSpace(this.txtAssunto.Text))
                Email.Assunto = this.txtAssunto.Text.Trim();

            return Email;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    classes.EmailEnvio Email = ObterEmailEnvio();

                    var usuario = new ManterUsuario().ObterUsuarioLogado();

                    if (usuario.IsGestor())
                        Email.Uf = usuario.UF;

                    ManterEmailEnvio manterEmailEnvio = new ManterEmailEnvio();
                    IList<classes.EmailEnvio> ListaEmail = manterEmailEnvio.ObterEmailEnvioPorFiltro(Email);
                    WebFormHelper.PreencherGrid(ListaEmail, this.dgvEmailEnvio);

                    if (ListaEmail != null && ListaEmail.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaEmail, this.dgvEmailEnvio);
                        pnlEmailEnvio.Visible = true;
                    }
                    else
                    {
                        pnlEmailEnvio.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void dgvEmailEnvio_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (!new ManterUsuario().ObterUsuarioLogado().ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Administrador))
            {
                ((DataControlField)dgvEmailEnvio.Columns
            .Cast<DataControlField>()
            .Where(x => x.HeaderText == "Uf")
            .SingleOrDefault()).Visible = false;
            }
        }
    }
}