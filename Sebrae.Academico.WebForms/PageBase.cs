using System.Collections.Generic;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Classe base para todas as páginas com acesso restrito.
    /// </summary>
    public abstract class PageBase : Page
    {
        /// <summary>
        /// Instancia o método VerificarPermissao() ao acessar a classe
        /// </summary>
        protected PageBase()
        {
            //VerificarPermissao();
        }

        /// <summary>
        /// Lista de Permissões necessárias para acessar uma funcionalidade
        /// </summary>
        protected abstract IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade { get; }

        /// <summary>
        /// Propriedade referente à Funcionalidade (Tela)
        /// </summary>
        protected abstract enumFuncionalidade Funcionalidade { get; }
               
        /// <summary>
        /// Verifica se o usuario logado possui o perfil necessário para acessar uma funcionalidade (tela)
        /// </summary>
        protected void VerificarPermissao()
        {
            bool temPermissao = new ManterUsuario().VerificarPermissao(this.PerfisNecessariosParaAcessarAFuncionalidade);

            if (!temPermissao)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/acessonegado.aspx");
            }
        }

        protected void MostrarTab(LinkButton link, HtmlGenericControl controle)
        {
            if (!string.IsNullOrWhiteSpace(link.Attributes["class"]))
            {
                //Exibir
                this.ExibirTab(link, controle);
            }
        }

        protected void OcultarTab(LinkButton link, HtmlGenericControl controle)
        {
            if (string.IsNullOrWhiteSpace(link.Attributes["class"]))
            {
                //Esconder
                this.EsconderTab(link, controle);
            }
        }

        protected bool AlterarStatusTab(LinkButton link, HtmlGenericControl controle)
        {
            if (string.IsNullOrWhiteSpace(link.Attributes["class"]))
            {
                //Esconder
                this.EsconderTab(link, controle);
                return false;
            }
            else
            {
                //Exibir
                this.ExibirTab(link, controle);
                return true;
            }
        }

        protected void ExibirTab(LinkButton link, HtmlGenericControl controle)
        {
            link.Attributes.Remove("class");
            controle.Attributes.Remove("class");
            controle.Attributes.Add("class", "panel-collapse in");
        }

        protected void EsconderTab(LinkButton link, HtmlGenericControl controle)
        {
            link.Attributes.Add("class", "collapsed");
            controle.Attributes.Remove("class");
            controle.Attributes.Add("class", "panel-collapse collapse");
        }

        /// <summary>
        /// Loga o acesso a uma determinada funcionalidade
        /// </summary>
        protected void LogarAcessoFuncionalidade()
        {
            ManterLogAcessoFuncionalidade manterLogAcessoFuncionalidade = new ManterLogAcessoFuncionalidade();
            LogAcessoFuncionalidade logAcessoFuncionalidade = this.ObterObjetoLogAcessoFuncionalidade();
            manterLogAcessoFuncionalidade.IncluirLogAcessoFuncionalidade(logAcessoFuncionalidade);
        }

        private LogAcessoFuncionalidade ObterObjetoLogAcessoFuncionalidade()
        {
            LogAcessoFuncionalidade logAcessoFuncionalidade = new LogAcessoFuncionalidade();
            logAcessoFuncionalidade.IDFuncionalidade = (int)this.Funcionalidade;
            logAcessoFuncionalidade.DataAcesso = System.DateTime.Now;
            Usuario usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            logAcessoFuncionalidade.IDUsuario = usuarioLogado.ID;
            return logAcessoFuncionalidade;
        }



        public void ExibirBackDrop()
        {
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = true;
        }

        public void OcultarBackDrop()
        {
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = false;
        }
    }
}