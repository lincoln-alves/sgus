using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms
{
    public partial class NovaSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    string token = Request.QueryString["token"];

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        //Verifica se o token ainda está válido
                        ManterSolicitacaoSenha manterSolicitacaoSenha = new ManterSolicitacaoSenha();
                        manterSolicitacaoSenha.VerificarVigenciaDoToken(token);

                        ViewState.Add("token", token);
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx");
                    }

                }
                catch
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        private Usuario ObterObjetoUsuario()
        {
            Usuario usuario = new Usuario();

            //CPF
            if (!string.IsNullOrWhiteSpace(this.txtCPF.Text))
            {
                usuario.CPF = this.txtCPF.Text;
            }

            //Senha
            if (!string.IsNullOrWhiteSpace(this.txtNovaSenha.Text))
            {
                usuario.Senha = this.txtNovaSenha.Text;
            }

            //Confirmar Senha
            if (!string.IsNullOrWhiteSpace(this.txtConfNovaSenha.Text))
            {
                usuario.ConfirmarSenhaLms = this.txtConfNovaSenha.Text;
            }

            return usuario;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = this.ObterObjetoUsuario();
                string token = ViewState["token"].ToString();
                ManterUsuario manterUsuario = new ManterUsuario();
                manterUsuario.RecuperarSenha(usuario, token);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Senha Alterada Com Sucesso !");

                //Todo: -> Autenticar o usuário no sistema

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
    }
}