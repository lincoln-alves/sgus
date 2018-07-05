using System;
using System.Web.UI;
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Cadastros.SincronizarMoodle
{
    public partial class ListarCadastros : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UserSelectedHandler(object sender, CompleteUserSelectionEvent e)
        {
            if (LupaUsuario.SelectedUser != null)
            {
                //Usuario usuario = LupaUsuario.SelectedUser;
                //this.IdUsuario = LupaUsuario.SelectedUser.ID;
                //IList<LogAcesso> LogsDeAcesso = new ManterLogAcesso().ObterUltimosAcessoDosUsuario(usuario.ID);
                //WebFormHelper.PreencherGrid(LogsDeAcesso, this.dgvLogAcessosDoUsuario);
                ////this.PreencherPainelComInformacoesDoUsuario(usuario);
                //this.IdUsuario = usuario.ID;
                //this.CPFUsuario = usuario.CPF;
                ////this.HabilitarBotaoNotificacoes();
                //pnlGerenciador.Visible = true;
                //this.ExibirPaineis();
                //this.ExibirPanelDadosPessoais(usuario);
                //this.btnHistorico.Visible = true;
            }
            else
            {
                //this.btnHistorico.Visible = false;
            }
        }
    }
}