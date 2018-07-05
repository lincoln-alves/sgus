using System;
using System.Web.UI;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Turma
{
    public partial class EdicaoTurma1 : Page
    {
        private Dominio.Classes.Turma turmaEdicao = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucTurma1.EsconderBotaoSalvar();
                ucTurma1.EsconderTitulo();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["Republicar"]))
                {
                    ucTurma1.RepublicarTurma();

                }
                else
                {
                    ucTurma1.SalvarTurma();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro no salvamento da Turma. Mensagem técnica do erro:" + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarTurma.aspx");
        }
    }
}