using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Web;

namespace Sebrae.Academico.WebForms
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["logout"] != null)
            {
                Session.Clear();

                //Remover Cookie de Usuario Logado.
                if (Request.Cookies["usuarioLogado"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("usuarioLogado");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }
                Response.Redirect("~/Default.aspx");
            }

            if(!Page.IsPostBack)
            {
                //#858 - Recuperando usuário do cookie
                if (!new ManterUsuario().EstaLogado() && Request.Cookies["usuarioLogado"] != null &&
                    !string.IsNullOrEmpty(Request.Cookies["usuarioLogado"].Value.ToString()))
                {
                    int id;
                    if (int.TryParse(Request.Cookies["usuarioLogado"].Value.ToString(), out id))
                    {
                        new BM.Classes.BMUsuario().SetarUsuarioLogado(new ManterUsuario().ObterUsuarioPorID(id));
                    }
                }

                if (new ManterUsuario().EstaLogado())
                {
                    string url = Request.QueryString["ReturnUrl"];
                    Response.Redirect(url ?? "/Dashboard.aspx");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //Usuário
                string usuario = string.Empty;
                string senha = string.Empty;

                if (!string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    usuario = txtUsuario.Text.Trim().Replace(".", "").Replace("-", "");
                }

                //Senha
                if (!string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    senha = txtSenha.Text.Trim();
                }

                classes.Usuario usuariologado = new ManterUsuario().Login(usuario, senha, true);

                if (new ManterUsuario().EstaLogado())
                {
                    //#858 - Adicionando usuário no cookie
                    if (Response.Cookies["usuarioLogado"] == null || string.IsNullOrEmpty(Response.Cookies["usuarioLogado"].Value))
                    {
                        if (Response.Cookies["usuarioLogado"] != null)
                        {
                            Response.Cookies["usuarioLogado"].Value = usuariologado.ID.ToString();
                            Response.Cookies["usuarioLogado"].Expires = DateTime.Now.AddHours(4);
                        }
                        else
                        {
                            var cookie = new HttpCookie("usuarioLogado");
                            cookie.Value = usuariologado.ID.ToString();
                            cookie.Expires = DateTime.Now.AddHours(4);
                            Response.Cookies.Add(cookie);
                        }
                    }

                    string url = Request.QueryString["ReturnUrl"];
                    Response.Redirect(url ?? "/Dashboard.aspx");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao processar a solicitação");
            }
        }


    }
}