using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class AlterarSenha : PageBase
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    base.LogarAcessoFuncionalidade();
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
                        btnAlterarSenha.Visible = false;
                        throw new AcademicoException("Nenhum Token foi encontrado. A senha não pode ser alterada");
                        
                    }

                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.AtividadeFormativa; }
        }

        private Dominio.Classes.Usuario ObterObjetoUsuario()
        {
            Dominio.Classes.Usuario usuario = new Dominio.Classes.Usuario();

            if (!string.IsNullOrWhiteSpace(this.txtNovaSenha.Text))
            {
                usuario.Senha = this.txtNovaSenha.Text.Trim();
                usuario.ConfirmarSenhaLms = this.txtConfNovaSenha.Text.Trim();
            }

            return usuario;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ManterUsuario manterUsuario = new ManterUsuario();
                Usuario usuario = this.ObterObjetoUsuario();
                string token = ViewState["token"].ToString();
                manterUsuario.AlterarSenha(usuario, token);

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