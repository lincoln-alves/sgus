using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;


namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ObterSenhaEmergencia : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();
                    lblDataExpiracao.Visible = false;
                    lblSenhaEmergencia.Visible = false;
                    btnGerar.Visible = true;
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
            get { return enumFuncionalidade.SenhaEmergencia; }
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {
            SenhaEmergencia se = (new ManterSenhaEmergencia()).ObterNovaSenhaEmergencia();
            lblSenhaEmergencia.Text = se.Senha;
            lblDataExpiracao.Text = se.DataValidade.ToString("dd/MM/yyyy hh:mm:ss");

            lblDataExpiracao.Visible = true;
            lblSenhaEmergencia.Visible = true;
            btnGerar.Visible = false;
        }
    }
}