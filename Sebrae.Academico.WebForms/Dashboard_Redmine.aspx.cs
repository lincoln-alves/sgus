using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web.UI.HtmlControls;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Tela de Dashboard.
    /// </summary>
    public partial class Dashboard_Redmine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var enderecoDashboardRedmine = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DashboardRedmine).Registro;

            //HtmlIframe FrameDashboard = (HtmlIframe)Page.FindControl("FrameDashboard");
            FrameDashboard.Attributes.Add("src", enderecoDashboardRedmine);
        }
    }
}
