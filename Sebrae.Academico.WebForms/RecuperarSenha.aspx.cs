using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms
{
    public partial class RecuperarSenha : System.Web.UI.Page
    {

        //private ManterUsuario manterUsuario = null;

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ManterUsuario manterUsuario = new ManterUsuario();
                manterUsuario.ProcessarRecuperacaoSenhaComConfirmacao(txtCPF.Text);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Enviamos um e-mail com os dados para recuperação da senha");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
    }
}