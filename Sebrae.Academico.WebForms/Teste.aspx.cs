using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Classes.ConheciGame;
using Sebrae.Academico.Dominio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms
{
    public partial class Teste : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(1433);
        }

        protected void lnkText_Click(object sender, EventArgs e)
        {
            txtTeste.Text = "deu trigger !";
        }
    }
}