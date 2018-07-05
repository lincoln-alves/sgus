using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        public string UrlListar { get; set; }
        public string UrlAdicionar { get; set; }
        public string UrlConfigurar { get; set; }
        public string UrlGerenciar { get; set; }
        public string UrlRelatar { get; set; } 
        public string Nome { get; set; }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            hypLinkMenuNome.Text = Nome;
         
            if (string.IsNullOrEmpty(UrlAdicionar))
            {
                hypLinkMenuAdicionar.Visible = false;
            }
            else
            {
                hypLinkMenuAdicionar.NavigateUrl = UrlAdicionar;
                hypLinkMenuNome.NavigateUrl = UrlAdicionar;
            }

            if (string.IsNullOrEmpty(UrlConfigurar))
            {
                hypLinkMenuConfigurar.Visible = false;
            }
            else
            {
                hypLinkMenuConfigurar.NavigateUrl = UrlConfigurar;
                hypLinkMenuNome.NavigateUrl = UrlConfigurar;
            }

            if (string.IsNullOrEmpty(UrlGerenciar))
            {
                hypLinkMenuGerenciar.Visible = false;
            }
            else
            {
                hypLinkMenuGerenciar.NavigateUrl = UrlGerenciar;
                hypLinkMenuNome.NavigateUrl = UrlGerenciar;
            }

            if (string.IsNullOrEmpty(UrlRelatar))
            {
                hypLinkMenuRelatar.Visible = false;
            }
            else
            {
                hypLinkMenuRelatar.NavigateUrl = UrlRelatar;
                hypLinkMenuNome.NavigateUrl = UrlRelatar;
            }

            if (string.IsNullOrEmpty(UrlListar))
            {
                hypLinkMenuListar.Visible = false;
            }else{
                hypLinkMenuListar.NavigateUrl = UrlListar;
                hypLinkMenuNome.NavigateUrl = UrlListar;
            }
            
        }
    }
}