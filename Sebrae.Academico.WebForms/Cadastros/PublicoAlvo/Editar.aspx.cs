using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.PublicoAlvo
{
    public partial class Editar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request["Id"] != null)
            {
                MontaTela();
            }
        }

        private void MontaTela()
        {
            var manterPublicoAlvo = new ManterPublicoAlvo();

            var publicoAlvo = manterPublicoAlvo.ObterPorID(int.Parse(Request["Id"]));

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsGestor() && (publicoAlvo.UF == null || usuarioLogado.UF.ID != publicoAlvo.UF.ID))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                    "Você não pode alterar Públicos-Alvo que não pertençam à sua UF.");
                Response.Redirect("Lista.aspx");
            }

            txtNome.Text = publicoAlvo.Nome;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                var manterPublicoAlvo = new ManterPublicoAlvo();

                var publicoAlvo = ObterObjetoPublicoAlvo(manterPublicoAlvo);

                new ManterPublicoAlvo().Salvar(publicoAlvo);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados cadastrados com sucesso", "Lista.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtNome.Text)) throw new AcademicoException("Nome é obrigatório.");
        }

        private classes.PublicoAlvo ObterObjetoPublicoAlvo(ManterPublicoAlvo manterPublicoAlvo = null)
        {
            if(manterPublicoAlvo == null)manterPublicoAlvo = new ManterPublicoAlvo();

            classes.PublicoAlvo publicoAlvo;

            if (Request["Id"] != null)
            {
                publicoAlvo = manterPublicoAlvo.ObterPorID(int.Parse(Request["Id"]));
            }
            else
            {
                publicoAlvo = new classes.PublicoAlvo();
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                if (usuarioLogado.IsGestor())
                    publicoAlvo.UF = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);
            }

            publicoAlvo.Auditoria = new classes.Auditoria(new ManterUsuario().ObterUsuarioLogado().CPF);
            publicoAlvo.Nome = txtNome.Text;

            return publicoAlvo;
        }
    }
}